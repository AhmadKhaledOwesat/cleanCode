using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RequestNumberff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GpsLocation",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GpsLocation",
                table: "AppUser");
        }
    }
}
