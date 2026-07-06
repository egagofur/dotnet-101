namespace WarehouseApi.Models;

public class Products
{
    public Guid Id { get; set; }
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<InventoryMovements> Movements { get; set; } = new List<InventoryMovements>();
    public ICollection<DailyStockReports> DailyReports { get; set; } = new List<DailyStockReports>();
}
