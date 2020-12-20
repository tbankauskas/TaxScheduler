using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Taxes.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipality",
                columns: table => new
                {
                    MunicipalityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.MunicipalityId);
                });

            migrationBuilder.CreateTable(
                name: "TaxType",
                columns: table => new
                {
                    TaxTypeId = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxType", x => x.TaxTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TaxScheduler",
                columns: table => new
                {
                    TaxSchedulerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    TaxTypeId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Week = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "Datetime", nullable: true),
                    TaxValue = table.Column<decimal>(type: "decimal(2,2)", precision: 2, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxScheduler", x => x.TaxSchedulerId);
                    table.ForeignKey(
                        name: "FK_TaxScheduler_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipality",
                        principalColumn: "MunicipalityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxScheduler_TaxType_TaxTypeId",
                        column: x => x.TaxTypeId,
                        principalTable: "TaxType",
                        principalColumn: "TaxTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_Name",
                table: "Municipality",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxScheduler_MunicipalityId",
                table: "TaxScheduler",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxScheduler_TaxTypeId",
                table: "TaxScheduler",
                column: "TaxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxType_TypeName",
                table: "TaxType",
                column: "TypeName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxScheduler");

            migrationBuilder.DropTable(
                name: "Municipality");

            migrationBuilder.DropTable(
                name: "TaxType");
        }
    }
}
