using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coravel.Queuing.Interfaces;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using WarehouseApi.Data;
using WarehouseApi.DTOs;
using WarehouseApi.Models;
using WarehouseApi.Enums;
using WarehouseApi.BackgroundJobs;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MovementsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IQueue _queue;
    private readonly IProductService _productService;

    public MovementsController(AppDbContext context, IQueue queue, IProductService productService)
    {
        _context = context;
        _queue = queue;
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateMovementRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "unknown@wms.com";

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("User ID tidak valid dalam token.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Produk dengan ID '{request.ProductId}' tidak ditemukan.");
            }

            // Get or create a default warehouse & location to satisfy foreign key constraints
            var location = await _context.WarehouseLocations.FirstOrDefaultAsync();
            if (location == null)
            {
                var warehouse = await _context.Warehouses.FirstOrDefaultAsync();
                if (warehouse == null)
                {
                    warehouse = new Warehouses
                    {
                        Id = Guid.NewGuid(),
                        Code = "WH-DEFAULT",
                        Name = "Default Warehouse",
                        Address = "Default Address",
                        City = "Default City",
                        IsActive = true
                    };
                    await _context.Warehouses.AddAsync(warehouse);
                }

                location = new WarehouseLocations
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = warehouse.Id,
                    Code = "LOC-DEFAULT",
                    Zone = "A",
                    Rack = "1",
                    Bin = "1",
                    IsActive = true
                };
                await _context.WarehouseLocations.AddAsync(location);
                await _context.SaveChangesAsync();
            }

            // Update stock levels
            var stockLevel = await _context.StockLevels
                .FirstOrDefaultAsync(sl => sl.ProductId == product.Id && sl.WarehouseLocationId == location.Id);

            if (stockLevel == null)
            {
                stockLevel = new StockLevels
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    WarehouseLocationId = location.Id,
                    Quantity = 0
                };
                await _context.StockLevels.AddAsync(stockLevel);
            }

            if (request.Type == "IN")
            {
                stockLevel.Quantity += request.Quantity;
            }
            else if (request.Type == "OUT")
            {
                if (stockLevel.Quantity < request.Quantity)
                {
                    throw new InvalidOperationException($"Stok produk '{product.Name}' di lokasi '{location.Code}' tidak mencukupi. Sisa stok: {stockLevel.Quantity}");
                }
                stockLevel.Quantity -= request.Quantity;
            }
            stockLevel.UpdatedAt = DateTime.UtcNow;

            // Log stock movement
            var movementType = request.Type == "IN" ? MovementType.INBOUND : MovementType.OUTBOUND;
            var movement = new StockMovements
            {
                Id = Guid.NewGuid(),
                MovementNumber = "MV-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Type = movementType,
                Status = MovementStatus.COMPLETED,
                CreatedBy = userId,
                MovementDate = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.StockMovements.AddAsync(movement);

            // Add movement item
            var movementItem = new StockMovementItems
            {
                Id = Guid.NewGuid(),
                MovementId = movement.Id,
                ProductId = product.Id,
                SourceLocationId = request.Type == "OUT" ? location.Id : null,
                DestinationLocationId = request.Type == "IN" ? location.Id : null,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.StockMovementItems.AddAsync(movementItem);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var payload = new MovementPayload
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Sku = product.Sku,
                Quantity = request.Quantity,
                Type = request.Type,
                UserEmail = userEmail
            };

            _queue.QueueInvocableWithPayload<SendMovementNotificationJob, MovementPayload>(payload);

            return Ok($"Mutasi stok berhasil disimpan. Pekerjaan background notifikasi untuk SKU {product.Sku} telah diantrekan.");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
