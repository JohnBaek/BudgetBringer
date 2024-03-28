using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class _03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_DisplayName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DisplayName1",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "LoginId",
                table: "User",
                type: "VARCHAR(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "로그인아이디")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_LoginId",
                table: "User",
                column: "LoginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_LoginId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName1",
                table: "User",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_DisplayName",
                table: "User",
                column: "DisplayName");
        }
    }
}
