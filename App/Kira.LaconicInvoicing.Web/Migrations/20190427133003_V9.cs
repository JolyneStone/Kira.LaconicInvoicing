using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Inventory");

            migrationBuilder.AddColumn<int>(
                name: "GoodsCategory",
                table: "Inventory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InboundReceipt",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 255, nullable: false),
                    WaybillNo = table.Column<string>(maxLength: 255, nullable: true),
                    Supplier = table.Column<string>(maxLength: 255, nullable: false),
                    SupplierNo = table.Column<string>(maxLength: 255, nullable: true),
                    SupplyAddress = table.Column<string>(maxLength: 255, nullable: false),
                    Deliveryman = table.Column<string>(maxLength: 255, nullable: false),
                    DeliverymanContact = table.Column<string>(maxLength: 255, nullable: true),
                    Consignee = table.Column<string>(maxLength: 255, nullable: false),
                    ConsigneeContact = table.Column<string>(maxLength: 255, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(maxLength: 255, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundReceipt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InboundReceiptItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InboundReceiptId = table.Column<Guid>(nullable: false),
                    GoodsCategory = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_InboundReceiptItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboundReceipt",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 255, nullable: false),
                    WaybillNo = table.Column<string>(maxLength: 255, nullable: true),
                    Receiver = table.Column<string>(maxLength: 255, nullable: false),
                    ReceiverNo = table.Column<string>(maxLength: 255, nullable: true),
                    ReceiverAddress = table.Column<string>(maxLength: 255, nullable: false),
                    Deliveryman = table.Column<string>(maxLength: 255, nullable: false),
                    DeliverymanContact = table.Column<string>(maxLength: 255, nullable: true),
                    Consignor = table.Column<string>(maxLength: 255, nullable: false),
                    ConsignorContact = table.Column<string>(maxLength: 255, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(maxLength: 255, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundReceipt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboundReceiptItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OutboundReceiptId = table.Column<Guid>(nullable: false),
                    GoodsCategory = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_OutboundReceiptItem", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 21, 30, 2, 497, DateTimeKind.Local).AddTicks(9739));

            migrationBuilder.CreateIndex(
                name: "IX_InboundReceipt_Number",
                table: "InboundReceipt",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboundReceipt_Number",
                table: "OutboundReceipt",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboundReceipt");

            migrationBuilder.DropTable(
                name: "InboundReceiptItem");

            migrationBuilder.DropTable(
                name: "OutboundReceipt");

            migrationBuilder.DropTable(
                name: "OutboundReceiptItem");

            migrationBuilder.DropColumn(
                name: "GoodsCategory",
                table: "Inventory");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Inventory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 26, 20, 30, 10, 894, DateTimeKind.Local).AddTicks(240));
        }
    }
}
