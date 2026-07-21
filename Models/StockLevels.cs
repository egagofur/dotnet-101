using System;

namespace WarehouseApi.Models;

public class StockLevels : BaseModel
{
    public Guid ProductId { get; set; }
    public Products? Product { get; set; }

    public Guid WarehouseLocationId { get; set; }
    public WarehouseLocations? WarehouseLocation { get; set; }

    public int Quantity { get; set; }
}
