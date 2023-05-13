using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateTimeToDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Apartaments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Apartaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
