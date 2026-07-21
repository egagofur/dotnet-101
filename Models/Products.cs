using System;
using System.Collections.Generic;

namespace WarehouseApi.Models;

public class Products : BaseModel
{
    public Guid CategoryId { get; set; }
    public ProductCategories? Category { get; set; }

    public required string Sku { get; set; }
    public required string Name { get; set; }
    public required string Unit { get; set; }
    public decimal Weight { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<ProductSuppliers> ProductSuppliers { get; set; } = new List<ProductSuppliers>();
    public ICollection<StockLevels> StockLevels { get; set; } = new List<StockLevels>();
    public ICollection<StockMovementItems> MovementItems { get; set; } = new List<StockMovementItems>();
    public ICollection<DailyStockReportItems> DailyReportItems { get; set; } = new List<DailyStockReportItems>();
}
