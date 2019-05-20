using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliverymanContact",
                table: "OutboundReceipt",
                newName: "DeliveryManContact");

            migrationBuilder.RenameColumn(
                name: "DeliverymanContact",
                table: "InboundReceipt",
                newName: "DeliveryManContact");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 21, 41, 6, 560, DateTimeKind.Local).AddTicks(6960));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryManContact",
                table: "OutboundReceipt",
                newName: "DeliverymanContact");

            migrationBuilder.RenameColumn(
                name: "DeliveryManContact",
                table: "InboundReceipt",
                newName: "DeliverymanContact");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 21, 39, 14, 277, DateTimeKind.Local).AddTicks(699));
        }
    }
}
