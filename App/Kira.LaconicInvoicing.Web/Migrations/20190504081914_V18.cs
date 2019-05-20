using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Path = table.Column<string>(maxLength: 255, nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Path = table.Column<string>(maxLength: 255, nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintTemplate", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 5, 4, 16, 19, 13, 171, DateTimeKind.Local).AddTicks(4013));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileTemplate");

            migrationBuilder.DropTable(
                name: "PrintTemplate");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 5, 2, 14, 36, 6, 336, DateTimeKind.Local).AddTicks(9262));
        }
    }
}
