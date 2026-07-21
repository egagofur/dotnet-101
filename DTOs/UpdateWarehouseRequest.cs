using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

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
