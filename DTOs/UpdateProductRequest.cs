using System;
using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

public class UpdateProductRequest
{
    [Required(ErrorMessage = "CategoryId wajib diisi.")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Name wajib diisi.")]
    [StringLength(255, ErrorMessage = "Name maksimal 255 karakter.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Unit wajib diisi.")]
    [StringLength(50, ErrorMessage = "Unit maksimal 50 karakter.")]
    public required string Unit { get; set; }

    [Required(ErrorMessage = "Weight wajib diisi.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Weight harus lebih besar dari 0.")]
    public decimal Weight { get; set; }

    [Required(ErrorMessage = "Status IsActive wajib ditentukan.")]
    public required bool IsActive { get; set; }
}
