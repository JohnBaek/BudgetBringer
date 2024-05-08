using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class _202405081110 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseYearForStatistics",
                table: "BudgetPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseYearForStatistics",
                table: "BudgetApproved",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_BaseYearForStatistics",
                table: "BudgetPlans",
                column: "BaseYearForStatistics");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApproved_BaseYearForStatistics",
                table: "BudgetApproved",
                column: "BaseYearForStatistics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_BaseYearForStatistics",
                table: "BudgetPlans");

            migrationBuilder.DropIndex(
                name: "IX_BudgetApproved_BaseYearForStatistics",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "BaseYearForStatistics",
                table: "BudgetPlans");

            migrationBuilder.DropColumn(
                name: "BaseYearForStatistics",
                table: "BudgetApproved");
        }
    }
}
