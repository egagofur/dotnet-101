using Microsoft.EntityFrameworkCore;
using WarehouseApi.Models;
using WarehouseApi.Enums;
using System;

namespace WarehouseApi.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  public DbSet<Users> Users => Set<Users>();
  public DbSet<Products> Products => Set<Products>();
  public DbSet<Suppliers> Suppliers => Set<Suppliers>();
  public DbSet<ProductCategories> ProductCategories => Set<ProductCategories>();
  public DbSet<ProductSuppliers> ProductSuppliers => Set<ProductSuppliers>();
  public DbSet<WarehouseLocations> WarehouseLocations => Set<WarehouseLocations>();
  public DbSet<StockLevels> StockLevels => Set<StockLevels>();
  public DbSet<StockMovementItems> StockMovementItems => Set<StockMovementItems>();
  public DbSet<StockMovements> StockMovements => Set<StockMovements>();
  public DbSet<Warehouses> Warehouses => Set<Warehouses>();
  public DbSet<DailyStockReports> DailyStockReports => Set<DailyStockReports>();
  public DbSet<DailyStockReportItems> DailyStockReportItems => Set<DailyStockReportItems>();
  public DbSet<NotificationLogs> NotificationLogs => Set<NotificationLogs>();
  public DbSet<ExternalApiLogs> ExternalApiLogs => Set<ExternalApiLogs>();
  public DbSet<JobExecutions> JobExecutions => Set<JobExecutions>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // 1. Users
    modelBuilder.Entity<Users>(entity =>
    {
      entity.ToTable("users");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
      entity.HasIndex(e => e.Email).IsUnique();
      entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Role).IsRequired().HasConversion<string>().HasMaxLength(50);
      entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();
    });

    // 2. Warehouses
    modelBuilder.Entity<Warehouses>(entity =>
    {
      entity.ToTable("warehouses");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
      entity.HasIndex(e => e.Code).IsUnique();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Address).IsRequired();
      entity.Property(e => e.City).IsRequired().HasMaxLength(100);
      entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();
    });

    // 3. Warehouse Locations
    modelBuilder.Entity<WarehouseLocations>(entity =>
    {
      entity.ToTable("warehouse_locations");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.WarehouseId).IsRequired();
      entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
      entity.Property(e => e.Zone).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Rack).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Bin).HasMaxLength(50);
      entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.Warehouse)
                .WithMany(w => w.Locations)
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);
    });

    // 4. Product Categories
    modelBuilder.Entity<ProductCategories>(entity =>
    {
      entity.ToTable("product_categories");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Description);
      entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();
    });

    // 5. Suppliers
    modelBuilder.Entity<Suppliers>(entity =>
    {
      entity.ToTable("suppliers");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
      entity.HasIndex(e => e.Code).IsUnique();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Phone).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Address).IsRequired();
      entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();
    });

    // 6. Products
    modelBuilder.Entity<Products>(entity =>
    {
      entity.ToTable("products");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.CategoryId).IsRequired();
      entity.Property(e => e.Sku).IsRequired().HasMaxLength(100);
      entity.HasIndex(e => e.Sku).IsUnique();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Unit).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Weight).HasColumnType("decimal(18,2)");
      entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
    });

    // 7. Product Suppliers
    modelBuilder.Entity<ProductSuppliers>(entity =>
    {
      entity.ToTable("product_suppliers");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.ProductId).IsRequired();
      entity.Property(e => e.SupplierId).IsRequired();
      entity.Property(e => e.SupplierSku).IsRequired().HasMaxLength(100);
      entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)");
      entity.Property(e => e.IsDefault).IsRequired().HasDefaultValue(false);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(e => e.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);
    });

    // 8. Stock Levels
    modelBuilder.Entity<StockLevels>(entity =>
    {
      entity.ToTable("stock_levels");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.ProductId).IsRequired();
      entity.Property(e => e.WarehouseLocationId).IsRequired();
      entity.Property(e => e.Quantity).IsRequired().HasDefaultValue(0);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasIndex(e => new { e.ProductId, e.WarehouseLocationId }).IsUnique();

      entity.HasOne(e => e.Product)
                .WithMany(p => p.StockLevels)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(e => e.WarehouseLocation)
                .WithMany()
                .HasForeignKey(e => e.WarehouseLocationId)
                .OnDelete(DeleteBehavior.Cascade);
    });

    // 9. Stock Movements
    modelBuilder.Entity<StockMovements>(entity =>
    {
      entity.ToTable("stock_movements");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.MovementNumber).IsRequired().HasMaxLength(100);
      entity.HasIndex(e => e.MovementNumber).IsUnique();
      entity.Property(e => e.Type).IsRequired().HasConversion<string>().HasMaxLength(50);
      entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
      entity.Property(e => e.SupplierId);
      entity.Property(e => e.CreatedBy).IsRequired();
      entity.Property(e => e.ApprovedBy);
      entity.Property(e => e.Notes);
      entity.Property(e => e.MovementDate).IsRequired();
      entity.Property(e => e.CompletedAt);
      entity.Property(e => e.CancelledAt);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.Supplier)
                .WithMany()
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

      entity.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.Approver)
                .WithMany()
                .HasForeignKey(e => e.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);
    });

    // 10. Stock Movement Items
    modelBuilder.Entity<StockMovementItems>(entity =>
    {
      entity.ToTable("stock_movement_items");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.MovementId).IsRequired();
      entity.Property(e => e.ProductId).IsRequired();
      entity.Property(e => e.SourceLocationId);
      entity.Property(e => e.DestinationLocationId);
      entity.Property(e => e.Quantity).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.Movement)
                .WithMany(m => m.Items)
                .HasForeignKey(e => e.MovementId)
                .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(e => e.Product)
                .WithMany(p => p.MovementItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.SourceLocation)
                .WithMany()
                .HasForeignKey(e => e.SourceLocationId)
                .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.DestinationLocation)
                .WithMany()
                .HasForeignKey(e => e.DestinationLocationId)
                .OnDelete(DeleteBehavior.Restrict);
    });

    // 11. Job Executions
    modelBuilder.Entity<JobExecutions>(entity =>
    {
      entity.ToTable("job_executions");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.JobName).IsRequired().HasMaxLength(255);
      entity.Property(e => e.StartedAt).IsRequired();
      entity.Property(e => e.FinishedAt);
      entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
      entity.Property(e => e.ErrorMessage);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();
    });

    // 12. Daily Stock Reports
    modelBuilder.Entity<DailyStockReports>(entity =>
    {
      entity.ToTable("daily_stock_reports");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.ReportDate).IsRequired();
      entity.Property(e => e.GeneratedByJob).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.GeneratedByJobNavigation)
                .WithMany()
                .HasForeignKey(e => e.GeneratedByJob)
                .OnDelete(DeleteBehavior.Cascade);
    });

    // 13. Daily Stock Report Items
    modelBuilder.Entity<DailyStockReportItems>(entity =>
    {
      entity.ToTable("daily_stock_report_items");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.ReportId).IsRequired();
      entity.Property(e => e.ProductId).IsRequired();
      entity.Property(e => e.TotalStock).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.Report)
                .WithMany(r => r.Items)
                .HasForeignKey(e => e.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(e => e.Product)
                .WithMany(p => p.DailyReportItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
    });

    // 14. Notification Logs
    modelBuilder.Entity<NotificationLogs>(entity =>
    {
      entity.ToTable("notification_logs");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.MovementId).IsRequired();
      entity.Property(e => e.RecipientUserId).IsRequired();
      entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
      entity.Property(e => e.RetryCount).IsRequired().HasDefaultValue(0);
      entity.Property(e => e.Response);
      entity.Property(e => e.SentAt).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasOne(e => e.Movement)
                .WithMany()
                .HasForeignKey(e => e.MovementId)
                .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(e => e.Recipient)
                .WithMany()
                .HasForeignKey(e => e.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);
    });

    // 15. External API Logs
    modelBuilder.Entity<ExternalApiLogs>(entity =>
    {
      entity.ToTable("external_api_logs");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(255);
      entity.Property(e => e.RequestUrl).IsRequired().HasMaxLength(2000);
      entity.Property(e => e.RequestPayload);
      entity.Property(e => e.ResponsePayload);
      entity.Property(e => e.RetryCount).IsRequired().HasDefaultValue(0);
      entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();
    });

    // Seed Data
    var defaultCategoryId = Guid.Parse("99999999-9999-9999-9999-999999999999");
    modelBuilder.Entity<ProductCategories>().HasData(
        new ProductCategories
        {
          Id = defaultCategoryId,
          Name = "Electronics",
          Description = "Electronic devices and accessories",
          IsActive = true,
          CreatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc),
          UpdatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc)
        }
    );

    modelBuilder.Entity<Products>().HasData(
        new Products
        {
          Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
          CategoryId = defaultCategoryId,
          Sku = "PROD-001",
          Name = "Laptop Dell XPS 15",
          Unit = "PCS",
          Weight = 2.0m,
          IsActive = true,
          CreatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc),
          UpdatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc)
        },
        new Products
        {
          Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          CategoryId = defaultCategoryId,
          Sku = "PROD-002",
          Name = "Mouse Logitech MX Master 3S",
          Unit = "PCS",
          Weight = 0.15m,
          IsActive = true,
          CreatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc),
          UpdatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc)
        }
    );

    modelBuilder.Entity<Users>().HasData(
        new Users
        {
          Id = Guid.Parse("d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a"),
          Name = "System Admin",
          Email = "admin@wms.com",
          Password = BCrypt.Net.BCrypt.HashPassword("AdminPassword123!"),
          Role = UserRole.ADMIN,
          IsActive = true,
          CreatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc),
          UpdatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc)
        }
    );
  }
}
