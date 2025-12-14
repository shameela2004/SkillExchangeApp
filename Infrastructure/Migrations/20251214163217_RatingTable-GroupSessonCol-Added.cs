using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RatingTableGroupSessonColAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Ratings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GroupSessionId",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_GroupSessionId",
                table: "Ratings",
                column: "GroupSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_GroupSessions_GroupSessionId",
                table: "Ratings",
                column: "GroupSessionId",
                principalTable: "GroupSessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_GroupSessions_GroupSessionId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_GroupSessionId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "GroupSessionId",
                table: "Ratings");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
