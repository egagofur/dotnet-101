using System;

namespace WarehouseApi.DTOs;

public class ProductResponse
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public required string Unit { get; set; }
    public decimal Weight { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
