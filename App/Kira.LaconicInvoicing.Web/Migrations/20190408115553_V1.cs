using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kira.LaconicInvoicing.Web.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Debt = table.Column<double>(nullable: false),
                    ContactPerson = table.Column<string>(maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Address = table.Column<string>(maxLength: 255, nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 18, nullable: true),
                    Specs = table.Column<string>(maxLength: 255, nullable: true),
                    Brand = table.Column<string>(maxLength: 255, nullable: true),
                    Unit = table.Column<string>(maxLength: 255, nullable: true),
                    CostPrice = table.Column<double>(nullable: true),
                    WholesalePrice = table.Column<double>(nullable: true),
                    RetailPrice = table.Column<double>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    VendorId = table.Column<Guid>(nullable: false),
                    VendorName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Material_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 8, 19, 55, 53, 8, DateTimeKind.Local).AddTicks(1801));

            migrationBuilder.CreateIndex(
                name: "IX_BaseData_Type",
                table: "BaseData",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Material_Number",
                table: "Material",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Material_VendorId",
                table: "Material",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_Number",
                table: "Vendor",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseData");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2019, 4, 2, 10, 28, 6, 732, DateTimeKind.Local).AddTicks(9735));
        }
    }
}
