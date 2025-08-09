using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingAssistantAI.Migrations
{
    /// <inheritdoc />
    public partial class Mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YaratılmaTarihi = table.Column<DateTime>(name: "Yaratılma Tarihi", type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(name: "Guncelleme Tarihi", type: "datetime2", nullable: true),
                    SilmeTarihi = table.Column<DateTime>(name: "Silme Tarihi", type: "datetime2", nullable: true),
                    VeriDurumu = table.Column<int>(name: "Veri Durumu", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YaratılmaTarihi = table.Column<DateTime>(name: "Yaratılma Tarihi", type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(name: "Guncelleme Tarihi", type: "datetime2", nullable: true),
                    SilmeTarihi = table.Column<DateTime>(name: "Silme Tarihi", type: "datetime2", nullable: true),
                    VeriDurumu = table.Column<int>(name: "Veri Durumu", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserID = table.Column<int>(type: "int", nullable: true),
                    YaratılmaTarihi = table.Column<DateTime>(name: "Yaratılma Tarihi", type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(name: "Guncelleme Tarihi", type: "datetime2", nullable: true),
                    SilmeTarihi = table.Column<DateTime>(name: "Silme Tarihi", type: "datetime2", nullable: true),
                    VeriDurumu = table.Column<int>(name: "Veri Durumu", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_AppUsers_AppUserID",
                        column: x => x.AppUserID,
                        principalTable: "AppUsers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    YaratılmaTarihi = table.Column<DateTime>(name: "Yaratılma Tarihi", type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(name: "Guncelleme Tarihi", type: "datetime2", nullable: true),
                    SilmeTarihi = table.Column<DateTime>(name: "Silme Tarihi", type: "datetime2", nullable: true),
                    VeriDurumu = table.Column<int>(name: "Veri Durumu", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => new { x.OrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserID",
                table: "Orders",
                column: "AppUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
