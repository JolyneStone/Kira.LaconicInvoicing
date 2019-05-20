using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deliveryman",
                table: "OutboundReceipt",
                newName: "DeliveryMan");

            migrationBuilder.RenameColumn(
                name: "Deliveryman",
                table: "InboundReceipt",
                newName: "DeliveryMan");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 21, 39, 14, 277, DateTimeKind.Local).AddTicks(699));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryMan",
                table: "OutboundReceipt",
                newName: "Deliveryman");

            migrationBuilder.RenameColumn(
                name: "DeliveryMan",
                table: "InboundReceipt",
                newName: "Deliveryman");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 27, 21, 30, 2, 497, DateTimeKind.Local).AddTicks(9739));
        }
    }
}
