namespace WarehouseApi.DTOs;

public class MovementPayload
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Sku { get; set; }
    public int Quantity { get; set; }
    public required string Type { get; set; } // "IN" or "OUT"
    public required string UserEmail { get; set; }
}
