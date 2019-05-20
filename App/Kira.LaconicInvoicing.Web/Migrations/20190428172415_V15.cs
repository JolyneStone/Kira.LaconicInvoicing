using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: false),
                    Debt = table.Column<double>(nullable: false),
                    ContactPerson = table.Column<string>(maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Address = table.Column<string>(maxLength: 255, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(maxLength: 255, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: false),
                    CustomerNumber = table.Column<string>(maxLength: 18, nullable: false),
                    CustomerName = table.Column<string>(maxLength: 255, nullable: false),
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
                    table.PrimaryKey("PK_SaleOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleOrderItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SaleOrderId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_SaleOrderItem", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 29, 1, 24, 14, 294, DateTimeKind.Local).AddTicks(6039));

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Number",
                table: "Customer",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrder_Number",
                table: "SaleOrder",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerProduct");

            migrationBuilder.DropTable(
                name: "SaleOrder");

            migrationBuilder.DropTable(
                name: "SaleOrderItem");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 28, 20, 49, 58, 0, DateTimeKind.Local).AddTicks(323));
        }
    }
}
