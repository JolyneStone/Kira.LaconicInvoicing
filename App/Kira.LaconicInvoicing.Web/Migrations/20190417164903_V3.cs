using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Material_Vendor_VendorId",
            //    table: "Material");

            //migrationBuilder.DropIndex(
            //    name: "IX_Material_VendorId",
            //    table: "Material");

            migrationBuilder.CreateTable(
                name: "VendorMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VendorId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorMaterial", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 18, 0, 49, 3, 240, DateTimeKind.Local).AddTicks(1342));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorMaterial");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 11, 0, 46, 31, 876, DateTimeKind.Local).AddTicks(6508));

            //migrationBuilder.CreateIndex(
            //    name: "IX_Material_VendorId",
            //    table: "Material",
            //    column: "VendorId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Material_Vendor_VendorId",
            //    table: "Material",
            //    column: "VendorId",
            //    principalTable: "Vendor",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
