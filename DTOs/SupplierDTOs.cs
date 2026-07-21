using System;
using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

public class CreateSupplierRequest
{
    [Required(ErrorMessage = "Code wajib diisi.")]
    [StringLength(100, ErrorMessage = "Code maksimal 100 karakter.")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Name wajib diisi.")]
    [StringLength(255, ErrorMessage = "Name maksimal 255 karakter.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [StringLength(255, ErrorMessage = "Email maksimal 255 karakter.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Phone wajib diisi.")]
    [StringLength(50, ErrorMessage = "Phone maksimal 50 karakter.")]
    public required string Phone { get; set; }

    [Required(ErrorMessage = "Address wajib diisi.")]
    public required string Address { get; set; }
}

public class UpdateSupplierRequest
{
    [Required(ErrorMessage = "Name wajib diisi.")]
    [StringLength(255, ErrorMessage = "Name maksimal 255 karakter.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [StringLength(255, ErrorMessage = "Email maksimal 255 karakter.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Phone wajib diisi.")]
    [StringLength(50, ErrorMessage = "Phone maksimal 50 karakter.")]
    public required string Phone { get; set; }

    [Required(ErrorMessage = "Address wajib diisi.")]
    public required string Address { get; set; }

    [Required(ErrorMessage = "Status IsActive wajib ditentukan.")]
    public required bool IsActive { get; set; }
}

public class SupplierResponse
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
