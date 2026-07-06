using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

public class CreateMovementRequest
{
    [Required(ErrorMessage = "Product ID wajib diisi.")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Quantity wajib diisi.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity harus lebih besar dari 0.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Type wajib diisi.")]
    [RegularExpression("^(IN|OUT)$", ErrorMessage = "Type harus bernilai 'IN' atau 'OUT'.")]
    public required string Type { get; set; }
}
