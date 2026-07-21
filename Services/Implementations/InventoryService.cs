using Coravel.Queuing.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseApi.BackgroundJobs;
using WarehouseApi.Data;
using WarehouseApi.DTOs;
using WarehouseApi.Enums;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Services.Implementations;

public class InventoryService : IInventoryService
{
    private readonly AppDbContext _context;
    private readonly IStockMovementRepository _movementRepository;
    private readonly IQueue _queue;

    public InventoryService(
        AppDbContext context,
        IStockMovementRepository movementRepository,
        IQueue queue)
    {
        _context = context;
        _movementRepository = movementRepository;
        _queue = queue;
    }

    public async Task<IEnumerable<StockMovementResponse>> GetAllMovementsAsync()
    {
        var movements = await _movementRepository.GetAllAsync();
        return movements.Select(MapToResponse);
    }

    public async Task<StockMovementResponse> GetMovementByIdAsync(Guid id)
    {
        var movement = await _movementRepository.GetByIdAsync(id);
        if (movement == null)
        {
            throw new KeyNotFoundException($"Mutasi stok dengan ID '{id}' tidak ditemukan.");
        }
        return MapToResponse(movement);
    }

    public async Task<StockMovementResponse> CreateMovementAsync(Guid userId, string userEmail, StockMovementRequest request)
    {
        // 1. Validate Supplier (if provided)
        if (request.SupplierId.HasValue)
        {
            var supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == request.SupplierId.Value);
            if (!supplierExists)
            {
                throw new KeyNotFoundException($"Supplier dengan ID '{request.SupplierId.Value}' tidak ditemukan.");
            }
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 2. Create Stock Movement parent record
            var movement = new StockMovements
            {
                Id = Guid.NewGuid(),
                MovementNumber = "MV-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Type = request.Type,
                Status = MovementStatus.COMPLETED,
                SupplierId = request.SupplierId,
                CreatedBy = userId,
                Notes = request.Notes,
                MovementDate = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var itemsToLog = new List<StockMovementItems>();
            var notificationPayloads = new List<MovementPayload>();

            // 3. Process each item in the request
            foreach (var itemRequest in request.Items)
            {
                var product = await _context.Products.FindAsync(itemRequest.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Produk dengan ID '{itemRequest.ProductId}' tidak ditemukan.");
                }

                // Check locations depending on type
                if (request.Type == MovementType.INBOUND || request.Type == MovementType.TRANSFER)
                {
                    if (!itemRequest.DestinationLocationId.HasValue)
                    {
                        throw new ArgumentException($"DestinationLocationId wajib diisi untuk mutasi tipe '{request.Type}'.");
                    }
                    var destExists = await _context.WarehouseLocations.AnyAsync(l => l.Id == itemRequest.DestinationLocationId.Value);
                    if (!destExists)
                    {
                        throw new KeyNotFoundException($"Lokasi tujuan dengan ID '{itemRequest.DestinationLocationId.Value}' tidak ditemukan.");
                    }
                }

                if (request.Type == MovementType.OUTBOUND || request.Type == MovementType.TRANSFER)
                {
                    if (!itemRequest.SourceLocationId.HasValue)
                    {
                        throw new ArgumentException($"SourceLocationId wajib diisi untuk mutasi tipe '{request.Type}'.");
                    }
                    var sourceExists = await _context.WarehouseLocations.AnyAsync(l => l.Id == itemRequest.SourceLocationId.Value);
                    if (!sourceExists)
                    {
                        throw new KeyNotFoundException($"Lokasi asal dengan ID '{itemRequest.SourceLocationId.Value}' tidak ditemukan.");
                    }
                }

                // 4. Update Stock Levels
                if (request.Type == MovementType.INBOUND)
                {
                    var destLocId = itemRequest.DestinationLocationId!.Value;
                    var stockLevel = await _context.StockLevels
                        .FirstOrDefaultAsync(sl => sl.ProductId == product.Id && sl.WarehouseLocationId == destLocId);

                    if (stockLevel == null)
                    {
                        stockLevel = new StockLevels
                        {
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            WarehouseLocationId = destLocId,
                            Quantity = 0,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _context.StockLevels.AddAsync(stockLevel);
                    }

                    stockLevel.Quantity += itemRequest.Quantity;
                    stockLevel.UpdatedAt = DateTime.UtcNow;
                }
                else if (request.Type == MovementType.OUTBOUND)
                {
                    var sourceLocId = itemRequest.SourceLocationId!.Value;
                    var stockLevel = await _context.StockLevels
                        .FirstOrDefaultAsync(sl => sl.ProductId == product.Id && sl.WarehouseLocationId == sourceLocId);

                    if (stockLevel == null || stockLevel.Quantity < itemRequest.Quantity)
                    {
                        var currentQty = stockLevel?.Quantity ?? 0;
                        throw new InvalidOperationException($"Stok produk '{product.Name}' tidak mencukupi di lokasi asal. Sisa stok: {currentQty}, diminta: {itemRequest.Quantity}");
                    }

                    stockLevel.Quantity -= itemRequest.Quantity;
                    stockLevel.UpdatedAt = DateTime.UtcNow;
                }
                else if (request.Type == MovementType.TRANSFER)
                {
                    var sourceLocId = itemRequest.SourceLocationId!.Value;
                    var destLocId = itemRequest.DestinationLocationId!.Value;

                    // Deduct from source
                    var sourceStock = await _context.StockLevels
                        .FirstOrDefaultAsync(sl => sl.ProductId == product.Id && sl.WarehouseLocationId == sourceLocId);

                    if (sourceStock == null || sourceStock.Quantity < itemRequest.Quantity)
                    {
                        var currentQty = sourceStock?.Quantity ?? 0;
                        throw new InvalidOperationException($"Stok produk '{product.Name}' tidak mencukupi untuk ditransfer di lokasi asal. Sisa stok: {currentQty}, diminta: {itemRequest.Quantity}");
                    }

                    sourceStock.Quantity -= itemRequest.Quantity;
                    sourceStock.UpdatedAt = DateTime.UtcNow;

                    // Add to destination
                    var destStock = await _context.StockLevels
                        .FirstOrDefaultAsync(sl => sl.ProductId == product.Id && sl.WarehouseLocationId == destLocId);

                    if (destStock == null)
                    {
                        destStock = new StockLevels
                        {
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            WarehouseLocationId = destLocId,
                            Quantity = 0,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _context.StockLevels.AddAsync(destStock);
                    }

                    destStock.Quantity += itemRequest.Quantity;
                    destStock.UpdatedAt = DateTime.UtcNow;
                }

                // 5. Create Movement Item
                var item = new StockMovementItems
                {
                    Id = Guid.NewGuid(),
                    MovementId = movement.Id,
                    ProductId = product.Id,
                    SourceLocationId = itemRequest.SourceLocationId,
                    DestinationLocationId = itemRequest.DestinationLocationId,
                    Quantity = itemRequest.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                itemsToLog.Add(item);

                // Prepare Coravel notification payload
                notificationPayloads.Add(new MovementPayload
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Sku = product.Sku,
                    Quantity = itemRequest.Quantity,
                    Type = request.Type.ToString(),
                    UserEmail = userEmail
                });
            }

            await _context.StockMovementItems.AddRangeAsync(itemsToLog);
            await _movementRepository.AddAsync(movement);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // 6. Queue background notifications outside database transaction
            foreach (var payload in notificationPayloads)
            {
                _queue.QueueInvocableWithPayload<SendMovementNotificationJob, MovementPayload>(payload);
            }

            // Return response loaded with full navigations
            var savedMovement = await _movementRepository.GetByIdAsync(movement.Id);
            return MapToResponse(savedMovement!);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static StockMovementResponse MapToResponse(StockMovements movement)
    {
        return new StockMovementResponse
        {
            Id = movement.Id,
            MovementNumber = movement.MovementNumber,
            Type = movement.Type,
            Status = movement.Status,
            SupplierName = movement.Supplier?.Name,
            CreatorName = movement.Creator?.Name ?? "System",
            ApproverName = movement.Approver?.Name,
            Notes = movement.Notes,
            MovementDate = movement.MovementDate,
            CompletedAt = movement.CompletedAt,
            CancelledAt = movement.CancelledAt,
            CreatedAt = movement.CreatedAt,
            UpdatedAt = movement.UpdatedAt,
            Items = movement.Items.Select(i => new StockMovementItemResponse
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown",
                Sku = i.Product?.Sku ?? "Unknown",
                SourceLocationCode = i.SourceLocation?.Code,
                DestinationLocationCode = i.DestinationLocation?.Code,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
