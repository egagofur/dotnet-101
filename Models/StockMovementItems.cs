using System;

namespace WarehouseApi.Models;

public class StockMovementItems : BaseModel
{
    public Guid MovementId { get; set; }
    public StockMovements? Movement { get; set; }

    public Guid ProductId { get; set; }
    public Products? Product { get; set; }

    public Guid? SourceLocationId { get; set; }
    public WarehouseLocations? SourceLocation { get; set; }

    public Guid? DestinationLocationId { get; set; }
    public WarehouseLocations? DestinationLocation { get; set; }

    public int Quantity { get; set; }
}
