namespace WarehouseApi.Models;

public class DailyStockReports
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int StockSnapshot { get; set; }
    public DateOnly ReportDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public Products? Product { get; set; }
}
