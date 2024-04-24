using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class _20240424 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncludeInStatistics",
                table: "BudgetPlans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncludeInStatistics",
                table: "BudgetPlans");
        }
    }
}
