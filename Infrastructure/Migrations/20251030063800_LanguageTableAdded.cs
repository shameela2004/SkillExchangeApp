using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LanguageTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorAvailability_MentorProfile_MentorProfileId",
                table: "MentorAvailability");

            migrationBuilder.DropForeignKey(
                name: "FK_MentorProfile_Users_UserId",
                table: "MentorProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubscriptionPlan_PlanId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionPlan",
                table: "SubscriptionPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MentorProfile",
                table: "MentorProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MentorAvailability",
                table: "MentorAvailability");

            migrationBuilder.RenameTable(
                name: "SubscriptionPlan",
                newName: "SubscriptionPlans");

            migrationBuilder.RenameTable(
                name: "MentorProfile",
                newName: "MentorProfiles");

            migrationBuilder.RenameTable(
                name: "MentorAvailability",
                newName: "MentorAvailabilities");

            migrationBuilder.RenameIndex(
                name: "IX_MentorProfile_UserId",
                table: "MentorProfiles",
                newName: "IX_MentorProfiles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MentorAvailability_MentorProfileId",
                table: "MentorAvailabilities",
                newName: "IX_MentorAvailabilities_MentorProfileId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionPlans",
                table: "SubscriptionPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MentorProfiles",
                table: "MentorProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MentorAvailabilities",
                table: "MentorAvailabilities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLogout = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Proficiency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLogout = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLanguages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_LanguageId",
                table: "UserLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_UserId",
                table: "UserLanguages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MentorAvailabilities_MentorProfiles_MentorProfileId",
                table: "MentorAvailabilities",
                column: "MentorProfileId",
                principalTable: "MentorProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MentorProfiles_Users_UserId",
                table: "MentorProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubscriptionPlans_PlanId",
                table: "Subscriptions",
                column: "PlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorAvailabilities_MentorProfiles_MentorProfileId",
                table: "MentorAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_MentorProfiles_Users_UserId",
                table: "MentorProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubscriptionPlans_PlanId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserLanguages");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionPlans",
                table: "SubscriptionPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MentorProfiles",
                table: "MentorProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MentorAvailabilities",
                table: "MentorAvailabilities");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "SubscriptionPlans",
                newName: "SubscriptionPlan");

            migrationBuilder.RenameTable(
                name: "MentorProfiles",
                newName: "MentorProfile");

            migrationBuilder.RenameTable(
                name: "MentorAvailabilities",
                newName: "MentorAvailability");

            migrationBuilder.RenameIndex(
                name: "IX_MentorProfiles_UserId",
                table: "MentorProfile",
                newName: "IX_MentorProfile_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MentorAvailabilities_MentorProfileId",
                table: "MentorAvailability",
                newName: "IX_MentorAvailability_MentorProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionPlan",
                table: "SubscriptionPlan",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MentorProfile",
                table: "MentorProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MentorAvailability",
                table: "MentorAvailability",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MentorAvailability_MentorProfile_MentorProfileId",
                table: "MentorAvailability",
                column: "MentorProfileId",
                principalTable: "MentorProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MentorProfile_Users_UserId",
                table: "MentorProfile",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubscriptionPlan_PlanId",
                table: "Subscriptions",
                column: "PlanId",
                principalTable: "SubscriptionPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
