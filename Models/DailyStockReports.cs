using System;
using System.Collections.Generic;

namespace WarehouseApi.Models;

public class DailyStockReports : BaseModel
{
    public DateOnly ReportDate { get; set; }
    public Guid GeneratedByJob { get; set; }
    public JobExecutions? GeneratedByJobNavigation { get; set; }

    // Navigation property
    public ICollection<DailyStockReportItems> Items { get; set; } = new List<DailyStockReportItems>();
}
