using System;

namespace WarehouseApi.Models;

public class WarehouseLocations : BaseModel
{
    public Guid WarehouseId { get; set; }
    public Warehouses? Warehouse { get; set; }

    public required string Code { get; set; }
    public required string Zone { get; set; }
    public required string Rack { get; set; }
    public string? Bin { get; set; }
    public bool IsActive { get; set; } = true;
}
