using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Bookings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BokchesArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookingId = table.Column<int>(type: "INTEGER", nullable: true),
                    InActiveTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BokchesArchive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BokchesArchive_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BokchesArchive_BookingId",
                table: "BokchesArchive",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BokchesArchive");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Bookings");
        }
    }
}
