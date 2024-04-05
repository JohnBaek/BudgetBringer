using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class _05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryBusinessManagerBusinessUnit",
                columns: table => new
                {
                    CountryBusinessManagerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BusinessUnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryBusinessManagerBusinessUnit", x => new { x.CountryBusinessManagerId, x.BusinessUnitId });
                    table.ForeignKey(
                        name: "FK_CountryBusinessManagerBusinessUnit_BusinessUnits_BusinessUni~",
                        column: x => x.BusinessUnitId,
                        principalTable: "CountryBusinessManagerBusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryBusinessManagerBusinessUnit_CountryBusinessManagers_C~",
                        column: x => x.CountryBusinessManagerId,
                        principalTable: "CountryBusinessManagers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CountryBusinessManagerBusinessUnit_BusinessUnitId",
                table: "CountryBusinessManagerBusinessUnit",
                column: "BusinessUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryBusinessManagerBusinessUnit");
        }
    }
}
