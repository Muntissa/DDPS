using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationToAppartmant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reservation",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Apartaments");

            migrationBuilder.AddColumn<bool>(
                name: "Reservation",
                table: "Apartaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reservation",
                table: "Apartaments");

            migrationBuilder.AddColumn<bool>(
                name: "Reservation",
                table: "Bookings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Apartaments",
                type: "TEXT",
                nullable: true);
        }
    }
}
