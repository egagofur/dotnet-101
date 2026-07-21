using WarehouseApi.Enums;

namespace WarehouseApi.Models;

public class ExternalApiLogs : BaseModel
{
    public required string ServiceName { get; set; }
    public required string RequestUrl { get; set; }
    public string? RequestPayload { get; set; }
    public string? ResponsePayload { get; set; }
    public int RetryCount { get; set; }
    public NotificationStatus Status { get; set; }
}