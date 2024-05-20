using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class _202405200943 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveDateValue",
                table: "BudgetPlans");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "BudgetPlans");

            migrationBuilder.DropColumn(
                name: "IsApprovalDateValid",
                table: "BudgetPlans");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "BudgetPlans");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "BudgetPlans");

            migrationBuilder.DropColumn(
                name: "ApproveDateValue",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "IsApprovalDateValid",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "BudgetApproved");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "BudgetApproved");

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "BusinessUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "BusinessUnits");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ApproveDateValue",
                table: "BudgetPlans",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "BudgetPlans",
                type: "varchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovalDateValid",
                table: "BudgetPlans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "BudgetPlans",
                type: "varchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "BudgetPlans",
                type: "varchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ApproveDateValue",
                table: "BudgetApproved",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "BudgetApproved",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovalDateValid",
                table: "BudgetApproved",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "BudgetApproved",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "BudgetApproved",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "BudgetApproved",
                type: "varchar(4)",
                maxLength: 4,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
