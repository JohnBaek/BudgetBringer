using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class _06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sectors_ModId",
                table: "Sectors",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_RegId",
                table: "Sectors",
                column: "RegId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryBusinessManagers_ModId",
                table: "CountryBusinessManagers",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryBusinessManagers_RegId",
                table: "CountryBusinessManagers",
                column: "RegId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_ModId",
                table: "CostCenters",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_RegId",
                table: "CostCenters",
                column: "RegId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_ModId",
                table: "BusinessUnits",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_RegId",
                table: "BusinessUnits",
                column: "RegId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_ModId",
                table: "BudgetPlans",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_RegId",
                table: "BudgetPlans",
                column: "RegId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApproved_ModId",
                table: "BudgetApproved",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApproved_RegId",
                table: "BudgetApproved",
                column: "RegId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetApproved_Users_ModId",
                table: "BudgetApproved",
                column: "ModId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetApproved_Users_RegId",
                table: "BudgetApproved",
                column: "RegId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlans_Users_ModId",
                table: "BudgetPlans",
                column: "ModId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlans_Users_RegId",
                table: "BudgetPlans",
                column: "RegId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessUnits_Users_ModId",
                table: "BusinessUnits",
                column: "ModId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessUnits_Users_RegId",
                table: "BusinessUnits",
                column: "RegId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_Users_ModId",
                table: "CostCenters",
                column: "ModId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_Users_RegId",
                table: "CostCenters",
                column: "RegId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CountryBusinessManagers_Users_ModId",
                table: "CountryBusinessManagers",
                column: "ModId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CountryBusinessManagers_Users_RegId",
                table: "CountryBusinessManagers",
                column: "RegId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sectors_Users_ModId",
                table: "Sectors",
                column: "ModId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sectors_Users_RegId",
                table: "Sectors",
                column: "RegId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetApproved_Users_ModId",
                table: "BudgetApproved");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetApproved_Users_RegId",
                table: "BudgetApproved");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlans_Users_ModId",
                table: "BudgetPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlans_Users_RegId",
                table: "BudgetPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessUnits_Users_ModId",
                table: "BusinessUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessUnits_Users_RegId",
                table: "BusinessUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_Users_ModId",
                table: "CostCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_Users_RegId",
                table: "CostCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryBusinessManagers_Users_ModId",
                table: "CountryBusinessManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryBusinessManagers_Users_RegId",
                table: "CountryBusinessManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sectors_Users_ModId",
                table: "Sectors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sectors_Users_RegId",
                table: "Sectors");

            migrationBuilder.DropIndex(
                name: "IX_Sectors_ModId",
                table: "Sectors");

            migrationBuilder.DropIndex(
                name: "IX_Sectors_RegId",
                table: "Sectors");

            migrationBuilder.DropIndex(
                name: "IX_CountryBusinessManagers_ModId",
                table: "CountryBusinessManagers");

            migrationBuilder.DropIndex(
                name: "IX_CountryBusinessManagers_RegId",
                table: "CountryBusinessManagers");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_ModId",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_RegId",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_BusinessUnits_ModId",
                table: "BusinessUnits");

            migrationBuilder.DropIndex(
                name: "IX_BusinessUnits_RegId",
                table: "BusinessUnits");

            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_ModId",
                table: "BudgetPlans");

            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_RegId",
                table: "BudgetPlans");

            migrationBuilder.DropIndex(
                name: "IX_BudgetApproved_ModId",
                table: "BudgetApproved");

            migrationBuilder.DropIndex(
                name: "IX_BudgetApproved_RegId",
                table: "BudgetApproved");
        }
    }
}
