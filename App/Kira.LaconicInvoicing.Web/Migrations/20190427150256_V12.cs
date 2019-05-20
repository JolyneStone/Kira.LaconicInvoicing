using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WarehouseName",
                table: "OutboundReceipt",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseNumber",
                table: "OutboundReceipt",
                maxLength: 18,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseName",
                table: "InboundReceipt",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseNumber",
                table: "InboundReceipt",
                maxLength: 18,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 23, 2, 55, 248, DateTimeKind.Local).AddTicks(2608));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseName",
                table: "OutboundReceipt");

            migrationBuilder.DropColumn(
                name: "WarehouseNumber",
                table: "OutboundReceipt");

            migrationBuilder.DropColumn(
                name: "WarehouseName",
                table: "InboundReceipt");

            migrationBuilder.DropColumn(
                name: "WarehouseNumber",
                table: "InboundReceipt");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 21, 41, 6, 560, DateTimeKind.Local).AddTicks(6960));
        }
    }
}
