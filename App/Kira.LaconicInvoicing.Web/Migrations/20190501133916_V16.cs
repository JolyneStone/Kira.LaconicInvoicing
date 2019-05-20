using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 5, 1, 21, 39, 15, 649, DateTimeKind.Local).AddTicks(6699));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 29, 1, 24, 14, 294, DateTimeKind.Local).AddTicks(6039));
        }
    }
}
