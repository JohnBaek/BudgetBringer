using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class _202405211350 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "BudgetApproved");

            migrationBuilder.AddColumn<double>(
                name: "NotPoIssueAmount",
                table: "BudgetApproved",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PoIssueAmount",
                table: "BudgetApproved",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SpendingAndIssuePoAmount",
                table: "BudgetApproved",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotPoIssueAmount",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "PoIssueAmount",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "SpendingAndIssuePoAmount",
                table: "BudgetApproved");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "BudgetApproved",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
