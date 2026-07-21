using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseApi.Data;
using WarehouseApi.Models;
using WarehouseApi.Enums;

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

        // 1. Create a Job Execution record
        var jobExecution = new JobExecutions
        {
            Id = Guid.NewGuid(),
            JobName = "DailyStockReportJob",
            StartedAt = DateTime.UtcNow,
            Status = JobStatus.RUNNING,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.JobExecutions.AddAsync(jobExecution);
        await _context.SaveChangesAsync();

        try
        {
            var reportExists = await _context.DailyStockReports.AnyAsync(r => r.ReportDate == today);
            if (reportExists)
            {
                _logger.LogInformation("Laporan stok harian untuk tanggal {Date} sudah ada. Skip.", today);
                
                jobExecution.Status = JobStatus.SUCCESS;
                jobExecution.FinishedAt = DateTime.UtcNow;
                jobExecution.UpdatedAt = DateTime.UtcNow;
                _context.JobExecutions.Update(jobExecution);
                await _context.SaveChangesAsync();
                return;
            }

            // 2. Create the daily stock report parent record
            var report = new DailyStockReports
            {
                Id = Guid.NewGuid(),
                ReportDate = today,
                GeneratedByJob = jobExecution.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.DailyStockReports.AddAsync(report);

            // 3. Get total stock levels grouped by product
            var productStocks = await _context.Products
                .Select(p => new
                {
                    ProductId = p.Id,
                    TotalStock = p.StockLevels.Sum(sl => sl.Quantity)
                })
                .ToListAsync();

            // 4. Create daily stock report items
            var reportItems = productStocks.Select(ps => new DailyStockReportItems
            {
                Id = Guid.NewGuid(),
                ReportId = report.Id,
                ProductId = ps.ProductId,
                TotalStock = ps.TotalStock,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            await _context.DailyStockReportItems.AddRangeAsync(reportItems);

            // Update Job Execution status to success
            jobExecution.Status = JobStatus.SUCCESS;
            jobExecution.FinishedAt = DateTime.UtcNow;
            jobExecution.UpdatedAt = DateTime.UtcNow;
            _context.JobExecutions.Update(jobExecution);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Laporan stok harian berhasil dibuat untuk {Count} produk.", reportItems.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saat menjalankan DailyStockReportJob.");
            
            jobExecution.Status = JobStatus.FAILED;
            jobExecution.FinishedAt = DateTime.UtcNow;
            jobExecution.ErrorMessage = ex.Message;
            jobExecution.UpdatedAt = DateTime.UtcNow;
            _context.JobExecutions.Update(jobExecution);
            await _context.SaveChangesAsync();
        }
    }
}
