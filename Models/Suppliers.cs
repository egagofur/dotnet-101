using System.Collections.Generic;

namespace WarehouseApi.Models;

public class Suppliers : BaseModel
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<ProductSuppliers> ProductSuppliers { get; set; } = new List<ProductSuppliers>();
}