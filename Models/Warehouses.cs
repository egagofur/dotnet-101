using System.Collections.Generic;

namespace WarehouseApi.Models;

public class Warehouses : BaseModel
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<WarehouseLocations> Locations { get; set; } = new List<WarehouseLocations>();
}