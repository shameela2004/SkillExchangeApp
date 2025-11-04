using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SessionAndGroupSesionUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Groups_GroupId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_GroupId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sessions");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "GroupSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "GroupSessions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "GroupSessions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "GroupSessions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "GroupSessions");

            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "GroupSessions");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Sessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_GroupId",
                table: "Sessions",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Groups_GroupId",
                table: "Sessions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
