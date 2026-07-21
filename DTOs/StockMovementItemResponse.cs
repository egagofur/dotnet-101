using System;

namespace WarehouseApi.DTOs;

public class StockMovementItemResponse
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Sku { get; set; }
    public string? SourceLocationCode { get; set; }
    public string? DestinationLocationCode { get; set; }
    public int Quantity { get; set; }
}
