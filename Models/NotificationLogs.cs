using System;
using WarehouseApi.Enums;

namespace WarehouseApi.Models;

public class NotificationLogs : BaseModel
{
    public Guid MovementId { get; set; }
    public StockMovements? Movement { get; set; }

    public Guid RecipientUserId { get; set; }
    public Users? Recipient { get; set; }

    public NotificationStatus Status { get; set; }
    public int RetryCount { get; set; }
    public string? Response { get; set; }
    public DateTime SentAt { get; set; }
}
