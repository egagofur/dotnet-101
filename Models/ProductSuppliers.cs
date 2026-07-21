using System;

namespace WarehouseApi.Models;

public class ProductSuppliers : BaseModel
{
    public Guid ProductId { get; set; }
    public Products? Product { get; set; }

    public Guid SupplierId { get; set; }
    public Suppliers? Supplier { get; set; }

    public required string SupplierSku { get; set; }
    public decimal PurchasePrice { get; set; }
    public bool IsDefault { get; set; }
}