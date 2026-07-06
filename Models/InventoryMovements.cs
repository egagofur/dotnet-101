namespace WarehouseApi.Models;

public class InventoryMovements
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public required string Type { get; set; } // "IN" or "OUT"
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Products? Product { get; set; }
    public Users? User { get; set; }
}
