using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GroupTablerole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Sessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "GroupMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "GroupMembers");
        }
    }
}
