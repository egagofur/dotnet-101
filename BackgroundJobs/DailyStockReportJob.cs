using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarehouseApi.Data;
using WarehouseApi.Models;

namespace WarehouseApi.BackgroundJobs;

public class DailyStockReportJob : IInvocable
{
    private readonly AppDbContext _context;
    private readonly ILogger<DailyStockReportJob> _logger;

    public DailyStockReportJob(AppDbContext context, ILogger<DailyStockReportJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Invoke()
    {
        _logger.LogInformation("Memulai pembuatan laporan stok harian...");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var reportExists = await _context.DailyStockReports.AnyAsync(r => r.ReportDate == today);
        if (reportExists)
        {
            _logger.LogInformation("Laporan stok harian untuk tanggal {Date} sudah ada. Skip.", today);
            return;
        }

        var products = await _context.Products.ToListAsync();
        var reports = products.Select(p => new DailyStockReports
        {
            Id = Guid.NewGuid(),
            ProductId = p.Id,
            StockSnapshot = p.Stock,
            ReportDate = today,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        await _context.DailyStockReports.AddRangeAsync(reports);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Laporan stok harian berhasil dibuat untuk {Count} produk.", reports.Count);
    }
}
