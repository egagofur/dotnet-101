using System;
using System.Collections.Generic;

namespace WarehouseApi.DTOs;

public class WarehouseResponse
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<LocationResponse> Locations { get; set; } = new();
}
