using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BokchesArchive_Bookings_BookingId",
                table: "BokchesArchive");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BokchesArchive",
                table: "BokchesArchive");

            migrationBuilder.DropIndex(
                name: "IX_BokchesArchive_BookingId",
                table: "BokchesArchive");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "BokchesArchive");

            migrationBuilder.RenameTable(
                name: "BokchesArchive",
                newName: "BookingsArchive");

            migrationBuilder.AddColumn<int>(
                name: "ApartamentNumber",
                table: "BookingsArchive",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingEndTime",
                table: "BookingsArchive",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingStartTime",
                table: "BookingsArchive",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ClientFirstName",
                table: "BookingsArchive",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientLastName",
                table: "BookingsArchive",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientSecondName",
                table: "BookingsArchive",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingsArchive",
                table: "BookingsArchive",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingsArchive",
                table: "BookingsArchive");

            migrationBuilder.DropColumn(
                name: "ApartamentNumber",
                table: "BookingsArchive");

            migrationBuilder.DropColumn(
                name: "BookingEndTime",
                table: "BookingsArchive");

            migrationBuilder.DropColumn(
                name: "BookingStartTime",
                table: "BookingsArchive");

            migrationBuilder.DropColumn(
                name: "ClientFirstName",
                table: "BookingsArchive");

            migrationBuilder.DropColumn(
                name: "ClientLastName",
                table: "BookingsArchive");

            migrationBuilder.DropColumn(
                name: "ClientSecondName",
                table: "BookingsArchive");

            migrationBuilder.RenameTable(
                name: "BookingsArchive",
                newName: "BokchesArchive");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "BokchesArchive",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BokchesArchive",
                table: "BokchesArchive",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BokchesArchive_BookingId",
                table: "BokchesArchive",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BokchesArchive_Bookings_BookingId",
                table: "BokchesArchive",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }
    }
}
