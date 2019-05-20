using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "Inventory",
                newName: "ItemId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Inventory",
                newName: "MaterialId");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 25, 15, 42, 2, 381, DateTimeKind.Local).AddTicks(7816));
        }
    }
}
