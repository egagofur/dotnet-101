using System;

namespace WarehouseApi.Models;

public class DailyStockReportItems : BaseModel
{
    public Guid ReportId { get; set; }
    public DailyStockReports? Report { get; set; }

    public Guid ProductId { get; set; }
    public Products? Product { get; set; }

    public int TotalStock { get; set; }
}
