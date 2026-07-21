using System;
using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.DTOs;

public class StockMovementItemRequest
{
    [Required(ErrorMessage = "ProductId wajib diisi.")]
    public Guid ProductId { get; set; }

    public Guid? SourceLocationId { get; set; }
    public Guid? DestinationLocationId { get; set; }

    [Required(ErrorMessage = "Quantity wajib diisi.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity harus lebih besar dari 0.")]
    public int Quantity { get; set; }
}
