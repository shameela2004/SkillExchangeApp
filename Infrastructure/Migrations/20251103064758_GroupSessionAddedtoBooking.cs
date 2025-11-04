using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GroupSessionAddedtoBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GroupSessionId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GroupSessionId",
                table: "Bookings",
                column: "GroupSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_GroupSessions_GroupSessionId",
                table: "Bookings",
                column: "GroupSessionId",
                principalTable: "GroupSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_GroupSessions_GroupSessionId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GroupSessionId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GroupSessionId",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
