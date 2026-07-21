using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WarehouseApi.Enums;

namespace WarehouseApi.DTOs;

public class StockMovementRequest
{
    [Required(ErrorMessage = "Type wajib diisi.")]
    public MovementType Type { get; set; }

    public Guid? SupplierId { get; set; }

    [StringLength(500, ErrorMessage = "Notes maksimal 500 karakter.")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Items mutasi wajib diisi.")]
    [MinLength(1, ErrorMessage = "Items mutasi minimal harus memiliki 1 produk.")]
    public List<StockMovementItemRequest> Items { get; set; } = new();
}

public class StockMovementItemRequest
{
    [Required(ErrorMessage = "ProductId wajib diisi.")]
    public Guid ProductId { get; set; }

    public Guid? SourceLocationId { get; set; }
    public Guid? DestinationLocationId { get; set; }

    [Required(ErrorMessage = "Quantity wajib diisi.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity harus lebih besar dari 0.")]
    public int Quantity { get; set; }
}

public class StockMovementResponse
{
    public Guid Id { get; set; }
    public required string MovementNumber { get; set; }
    public MovementType Type { get; set; }
    public MovementStatus Status { get; set; }
    public string? SupplierName { get; set; }
    public required string CreatorName { get; set; }
    public string? ApproverName { get; set; }
    public string? Notes { get; set; }
    public DateTime MovementDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<StockMovementItemResponse> Items { get; set; } = new();
}

public class StockMovementItemResponse
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Sku { get; set; }
    public string? SourceLocationCode { get; set; }
    public string? DestinationLocationCode { get; set; }
    public int Quantity { get; set; }
}
