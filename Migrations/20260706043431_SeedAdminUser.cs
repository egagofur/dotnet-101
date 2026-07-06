using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "Password", "RoleId", "Status", "UpdatedAt" },
                values: new object[] { new Guid("d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a"), new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), "admin@wms.com", "System Admin", "$2a$11$rF1zSvF0RVwQ7lEngkm7auxfGejAyNl3MPyoZ9u38hVOa6dPa6aUu", new Guid("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"), true, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a"));
        }
    }
}
