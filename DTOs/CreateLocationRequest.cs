using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

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
