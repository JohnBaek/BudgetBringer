using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class LogActionChangeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Contents",
                table: "LogActions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3000)",
                oldMaxLength: 3000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Contents",
                table: "LogActions",
                type: "varchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
