using System;
using System.Collections.Generic;
using WarehouseApi.Enums;

namespace WarehouseApi.DTOs;

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
