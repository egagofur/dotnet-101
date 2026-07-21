using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

public class CreateWarehouseRequest
{
    [Required(ErrorMessage = "Code wajib diisi.")]
    [StringLength(100, ErrorMessage = "Code maksimal 100 karakter.")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Name wajib diisi.")]
    [StringLength(255, ErrorMessage = "Name maksimal 255 karakter.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Address wajib diisi.")]
    public required string Address { get; set; }

    [Required(ErrorMessage = "City wajib diisi.")]
    [StringLength(100, ErrorMessage = "City maksimal 100 karakter.")]
    public required string City { get; set; }
}

public class UpdateWarehouseRequest
{
    [Required(ErrorMessage = "Name wajib diisi.")]
    [StringLength(255, ErrorMessage = "Name maksimal 255 karakter.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Address wajib diisi.")]
    public required string Address { get; set; }

    [Required(ErrorMessage = "City wajib diisi.")]
    [StringLength(100, ErrorMessage = "City maksimal 100 karakter.")]
    public required string City { get; set; }

    [Required(ErrorMessage = "Status IsActive wajib diisi.")]
    public required bool IsActive { get; set; }
}

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

public class CreateLocationRequest
{
    [Required(ErrorMessage = "Code lokasi wajib diisi.")]
    [StringLength(100, ErrorMessage = "Code maksimal 100 karakter.")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Zone wajib diisi.")]
    [StringLength(50, ErrorMessage = "Zone maksimal 50 karakter.")]
    public required string Zone { get; set; }

    [Required(ErrorMessage = "Rack wajib diisi.")]
    [StringLength(50, ErrorMessage = "Rack maksimal 50 karakter.")]
    public required string Rack { get; set; }

    [StringLength(50, ErrorMessage = "Bin maksimal 50 karakter.")]
    public string? Bin { get; set; }
}

public class LocationResponse
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Zone { get; set; }
    public required string Rack { get; set; }
    public string? Bin { get; set; }
    public bool IsActive { get; set; }
}
