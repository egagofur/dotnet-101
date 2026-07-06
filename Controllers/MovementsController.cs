using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coravel.Queuing.Interfaces;
using System.Security.Claims;
using WarehouseApi.Data;
using WarehouseApi.DTOs;
using WarehouseApi.Models;
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

            if (request.Type == "IN")
            {
                product.Stock += request.Quantity;
            }
            else if (request.Type == "OUT")
            {
                if (product.Stock < request.Quantity)
                {
                    throw new InvalidOperationException($"Stok produk '{product.Name}' tidak mencukupi. Sisa stok: {product.Stock}");
                }
                product.Stock -= request.Quantity;
            }

            product.UpdatedAt = DateTime.UtcNow;

            var movement = new InventoryMovements
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Quantity = request.Quantity,
                Type = request.Type,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.InventoryMovements.AddAsync(movement);
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
