using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Vendor",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 18,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Vendor",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Vendor",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Operator",
                table: "Vendor",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Vendor",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Material",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specs",
                table: "Material",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Material",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 18,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Material",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Operator",
                table: "Material",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Material",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 11, 0, 46, 31, 876, DateTimeKind.Local).AddTicks(6508));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Operator",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "Operator",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Material");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Vendor",
                maxLength: 18,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 18);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Vendor",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Material",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Specs",
                table: "Material",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Material",
                maxLength: 18,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 18);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 8, 19, 55, 53, 8, DateTimeKind.Local).AddTicks(1801));
        }
    }
}
