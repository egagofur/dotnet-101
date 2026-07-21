using System;
using WarehouseApi.Enums;

namespace WarehouseApi.Models;

public class JobExecutions : BaseModel
{
    public required string JobName { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public JobStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
