using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Alamat email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format alamat email tidak valid.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password wajib diisi.")]
    public required string Password { get; set; }
}
