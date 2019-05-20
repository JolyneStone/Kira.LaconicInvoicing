using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "RetailPrice",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "WholesalePrice",
                table: "Material");

            migrationBuilder.AlterColumn<string>(
                name: "Operator",
                table: "Vendor",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Operator",
                table: "Material",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Material",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Num = table.Column<int>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: false),
                    VendorNumber = table.Column<string>(maxLength: 18, nullable: false),
                    VendorName = table.Column<string>(maxLength: 255, nullable: false),
                    ConsignorContact = table.Column<string>(maxLength: 255, nullable: false),
                    Consignor = table.Column<string>(maxLength: 255, nullable: true),
                    SourceAddress = table.Column<string>(maxLength: 255, nullable: false),
                    DestAddress = table.Column<string>(maxLength: 255, nullable: false),
                    WarehouseNumber = table.Column<string>(maxLength: 255, nullable: false),
                    WarehouseName = table.Column<string>(maxLength: 255, nullable: false),
                    Consignee = table.Column<string>(maxLength: 255, nullable: true),
                    ConsigneeContact = table.Column<string>(maxLength: 255, nullable: false),
                    Freight = table.Column<double>(nullable: true),
                    TotalAmount = table.Column<double>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    AmountPaid = table.Column<double>(nullable: false),
                    PayWay = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(maxLength: 255, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PurchaseOrderId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: false),
                    Spec = table.Column<string>(maxLength: 255, nullable: false),
                    Brand = table.Column<string>(maxLength: 255, nullable: true),
                    Unit = table.Column<string>(maxLength: 255, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Manager = table.Column<string>(maxLength: 255, nullable: false),
                    ManagerContact = table.Column<string>(maxLength: 255, nullable: false),
                    Area = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(maxLength: 255, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 25, 15, 42, 2, 381, DateTimeKind.Local).AddTicks(7816));

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_Number",
                table: "PurchaseOrder",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_Number",
                table: "Warehouse",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItem");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Material");

            migrationBuilder.AlterColumn<string>(
                name: "Operator",
                table: "Vendor",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Operator",
                table: "Material",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CostPrice",
                table: "Material",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RetailPrice",
                table: "Material",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WholesalePrice",
                table: "Material",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 23, 16, 10, 22, 402, DateTimeKind.Local).AddTicks(270));
        }
    }
}
