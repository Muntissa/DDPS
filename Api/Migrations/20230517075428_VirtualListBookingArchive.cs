using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class VirtualListBookingArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingsArchiveId",
                table: "Services",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_BookingsArchiveId",
                table: "Services",
                column: "BookingsArchiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_BookingsArchive_BookingsArchiveId",
                table: "Services",
                column: "BookingsArchiveId",
                principalTable: "BookingsArchive",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_BookingsArchive_BookingsArchiveId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_BookingsArchiveId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "BookingsArchiveId",
                table: "Services");
        }
    }
}
