using WarehouseApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Nama wajib diisi.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nama harus terdiri dari 3 hingga 100 karakter.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Alamat email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format alamat email tidak valid.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password wajib diisi.")]
    [MinLength(8, ErrorMessage = "Password minimal harus memiliki 8 karakter.")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Role wajib dipilih.")]
    public required UserRole Role { get; set; }

    [Required(ErrorMessage = "Status wajib ditentukan.")]
    public required bool Status { get; set; } = true;
}
