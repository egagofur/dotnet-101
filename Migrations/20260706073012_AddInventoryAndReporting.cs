using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WarehouseApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryAndReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "daily_stock_reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSnapshot = table.Column<int>(type: "int", nullable: false),
                    ReportDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_stock_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_daily_stock_reports_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_movements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_movements_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_movements_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "CreatedAt", "Name", "Sku", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop Dell XPS 15", "PROD-001", 10, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Mouse Logitech MX Master 3S", "PROD-002", 50, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a"),
                column: "Password",
                value: "$2a$11$FDTKcK/LW/a0MwRGxasXC.ryziNpNQNGmOlPnf2SK1XFg.5uJ7Qkm");

            migrationBuilder.CreateIndex(
                name: "IX_daily_stock_reports_ProductId",
                table: "daily_stock_reports",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_ProductId",
                table: "inventory_movements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_UserId",
                table: "inventory_movements",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_stock_reports");

            migrationBuilder.DropTable(
                name: "inventory_movements");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a"),
                column: "Password",
                value: "$2a$11$rF1zSvF0RVwQ7lEngkm7auxfGejAyNl3MPyoZ9u38hVOa6dPa6aUu");
        }
    }
}
