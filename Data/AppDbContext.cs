using Microsoft.EntityFrameworkCore;
using WarehouseApi.Models;
using WarehouseApi.Enums;

namespace WarehouseApi.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  public DbSet<Users> Users => Set<Users>();
  public DbSet<Roles> Roles => Set<Roles>();

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
  }
}
