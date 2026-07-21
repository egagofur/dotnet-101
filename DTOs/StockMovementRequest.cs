using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WarehouseApi.Enums;

namespace WarehouseApi.DTOs;

public class StockMovementRequest
{
    [Required(ErrorMessage = "Type wajib diisi.")]
    public MovementType Type { get; set; }

    public Guid? SupplierId { get; set; }

    [StringLength(500, ErrorMessage = "Notes maksimal 500 karakter.")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Items mutasi wajib diisi.")]
    [MinLength(1, ErrorMessage = "Items mutasi minimal harus memiliki 1 produk.")]
    public List<StockMovementItemRequest> Items { get; set; } = new();
}
