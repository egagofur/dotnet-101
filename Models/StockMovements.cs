using System;
using System.Collections.Generic;
using WarehouseApi.Enums;

namespace WarehouseApi.Models;

public class StockMovements : BaseModel
{
    public required string MovementNumber { get; set; }
    public MovementType Type { get; set; }
    public MovementStatus Status { get; set; }

    public Guid? SupplierId { get; set; }
    public Suppliers? Supplier { get; set; }

    public Guid CreatedBy { get; set; }
    public Users? Creator { get; set; }

    public Guid? ApprovedBy { get; set; }
    public Users? Approver { get; set; }

    public string? Notes { get; set; }
    public DateTime MovementDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    // Navigation property
    public ICollection<StockMovementItems> Items { get; set; } = new List<StockMovementItems>();
}
