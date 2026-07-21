using System;

namespace WarehouseApi.DTOs;

public class LocationResponse
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Zone { get; set; }
    public required string Rack { get; set; }
    public string? Bin { get; set; }
    public bool IsActive { get; set; }
}
