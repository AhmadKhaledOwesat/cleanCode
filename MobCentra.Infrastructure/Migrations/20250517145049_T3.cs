using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class T3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotifyClient",
                table: "RequestStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotifyProvider",
                table: "RequestStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyClient",
                table: "RequestStatus");

            migrationBuilder.DropColumn(
                name: "NotifyProvider",
                table: "RequestStatus");
        }
    }
}
