using System.Collections.Generic;

namespace WarehouseApi.Models;

public class ProductCategories : BaseModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<Products> Products { get; set; } = new List<Products>();
}