using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperTrail.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addaudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Comments",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "isAudit",
                table: "Comments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "isAudit",
                table: "Comments");
        }
    }
}
