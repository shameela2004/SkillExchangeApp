using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MentorTablesExtendedFromBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MentorProfile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "MentorProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MentorProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MentorProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogout",
                table: "MentorProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "MentorProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastUpdatedBy",
                table: "MentorProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MentorAvailability",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "MentorAvailability",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MentorAvailability",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MentorAvailability",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogout",
                table: "MentorAvailability",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "MentorAvailability",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastUpdatedBy",
                table: "MentorAvailability",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MentorProfile");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "MentorProfile");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MentorProfile");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MentorProfile");

            migrationBuilder.DropColumn(
                name: "LastLogout",
                table: "MentorProfile");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "MentorProfile");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "MentorProfile");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MentorAvailability");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "MentorAvailability");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MentorAvailability");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MentorAvailability");

            migrationBuilder.DropColumn(
                name: "LastLogout",
                table: "MentorAvailability");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "MentorAvailability");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "MentorAvailability");
        }
    }
}
