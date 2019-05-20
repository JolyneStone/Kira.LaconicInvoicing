using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "ItemId",
            //    table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "Num",
                table: "Inventory",
                newName: "Amount");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Inventory",
                maxLength: 18,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: false),
                    Spec = table.Column<string>(maxLength: 255, nullable: false),
                    Brand = table.Column<string>(maxLength: 255, nullable: true),
                    Unit = table.Column<string>(maxLength: 255, nullable: false),
                    CostPrice = table.Column<double>(nullable: false),
                    WholesalePrice = table.Column<double>(nullable: false),
                    RetailPrice = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(maxLength: 255, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SourceWarehouseNumber = table.Column<string>(maxLength: 18, nullable: false),
                    SourceWarehouseName = table.Column<string>(maxLength: 255, nullable: false),
                    SourceAddress = table.Column<string>(maxLength: 255, nullable: false),
                    DestWarehouseNumber = table.Column<string>(maxLength: 18, nullable: false),
                    DestWarehouseName = table.Column<string>(maxLength: 255, nullable: false),
                    DestAddress = table.Column<string>(maxLength: 255, nullable: false),
                    Number = table.Column<string>(maxLength: 255, nullable: false),
                    Consignor = table.Column<string>(maxLength: 255, nullable: false),
                    ConsignorContact = table.Column<string>(maxLength: 255, nullable: true),
                    Consignee = table.Column<string>(maxLength: 255, nullable: false),
                    ConsigneeContact = table.Column<string>(maxLength: 255, nullable: true),
                    WaybillNo = table.Column<string>(maxLength: 255, nullable: true),
                    DeliveryMan = table.Column<string>(maxLength: 255, nullable: false),
                    DeliveryManContact = table.Column<string>(maxLength: 255, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(maxLength: 255, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrderItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransferOrderId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_TransferOrderItem", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 28, 19, 56, 7, 44, DateTimeKind.Local).AddTicks(6496));

            migrationBuilder.CreateIndex(
                name: "IX_Product_Number",
                table: "Product",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrder_Number",
                table: "TransferOrder",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "TransferOrder");

            migrationBuilder.DropTable(
                name: "TransferOrderItem");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Inventory",
                newName: "Num");

            //migrationBuilder.AddColumn<Guid>(
            //    name: "ItemId",
            //    table: "Inventory",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 23, 2, 55, 248, DateTimeKind.Local).AddTicks(2608));
        }
    }
}
