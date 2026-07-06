using Microsoft.EntityFrameworkCore;
using WarehouseApi.Models;
using WarehouseApi.Enums;

namespace WarehouseApi.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  public DbSet<Users> Users => Set<Users>();
  public DbSet<Roles> Roles => Set<Roles>();
  public DbSet<Products> Products => Set<Products>();
  public DbSet<InventoryMovements> InventoryMovements => Set<InventoryMovements>();
  public DbSet<DailyStockReports> DailyStockReports => Set<DailyStockReports>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Roles>(
      entity =>
      {
        entity.ToTable("roles");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.Name).IsRequired().HasConversion<string>().HasMaxLength(50);
        entity.Property(e => e.Description).HasMaxLength(500);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt).IsRequired();
      }
    );

    modelBuilder.Entity<Users>(
      entity =>
      {
        entity.ToTable("users");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Email).IsRequired();
        entity.Property(e => e.Password).IsRequired();
        entity.Property(e => e.RoleId).IsRequired();
        entity.Property(e => e.Status).IsRequired().HasDefaultValue(true);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt).IsRequired();

        entity.HasOne(e => e.Role)
              .WithMany(r => r.Users)
              .HasForeignKey(e => e.RoleId)
              .OnDelete(DeleteBehavior.Restrict);
      }
    );

    var adminRoleId = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d");
    var supervisorRoleId = Guid.Parse("b2c3d4e5-f6a7-8b9c-0d1e-2f3a4b5c6d7e");
    var operatorRoleId = Guid.Parse("c3d4e5f6-a7b8-9c0d-1e2f-3a4b5c6d7e8f");
    var adminUserId = Guid.Parse("d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a");

    modelBuilder.Entity<Users>().HasData(
    new Users
    {
      Id = adminUserId,
      Name = "System Admin",
      Email = "admin@wms.com",
      Password = BCrypt.Net.BCrypt.HashPassword("AdminPassword123!"), // Otomatis ter-hash saat migrasi dibuat
      RoleId = adminRoleId, // ID Role Admin yang sudah di-seed sebelumnya
      Status = true,
      CreatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc),
      UpdatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc)
    }
);

    modelBuilder.Entity<Roles>().HasData(
      new Roles
      {
        Id = adminRoleId,
        Name = RolesNameEnum.admin,
        Description = "Administrator",
        CreatedAt = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc)
      },
      new Roles
      {
        Id = supervisorRoleId,
        Name = RolesNameEnum.supervisor,
        Description = "Warehouse Supervisor",
        CreatedAt = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc)
      },
      new Roles
      {
        Id = operatorRoleId,
        Name = RolesNameEnum.warehouse_operator,
        Description = "Warehouse Operator",
        CreatedAt = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc)
      }
    );

    modelBuilder.Entity<Products>(entity =>
    {
      entity.ToTable("products");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.Sku).IsRequired().HasMaxLength(100);
      entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Stock).IsRequired().HasDefaultValue(0);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();
    });

    modelBuilder.Entity<InventoryMovements>(entity =>
    {
      entity.ToTable("inventory_movements");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.Type).IsRequired().HasMaxLength(10);
      entity.Property(e => e.Quantity).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();

      entity.HasOne(e => e.Product)
            .WithMany(p => p.Movements)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<DailyStockReports>(entity =>
    {
      entity.ToTable("daily_stock_reports");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedNever();
      entity.Property(e => e.StockSnapshot).IsRequired();
      entity.Property(e => e.ReportDate).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();

      entity.HasOne(e => e.Product)
            .WithMany(p => p.DailyReports)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<Products>().HasData(
      new Products
      {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Sku = "PROD-001",
        Name = "Laptop Dell XPS 15",
        Stock = 10,
        CreatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc)
      },
      new Products
      {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Sku = "PROD-002",
        Name = "Mouse Logitech MX Master 3S",
        Stock = 50,
        CreatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc)
      }
    );
  }
}
