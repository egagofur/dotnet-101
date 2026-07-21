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
