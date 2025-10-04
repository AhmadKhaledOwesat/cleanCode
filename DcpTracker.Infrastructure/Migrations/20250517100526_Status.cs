using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcpTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteAllowed",
                table: "RequestStatus");

            migrationBuilder.AddColumn<string>(
                name: "ClientAllowedStatus",
                table: "RequestStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderAllowedStatus",
                table: "RequestStatus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientAllowedStatus",
                table: "RequestStatus");

            migrationBuilder.DropColumn(
                name: "ProviderAllowedStatus",
                table: "RequestStatus");

            migrationBuilder.AddColumn<int>(
                name: "DeleteAllowed",
                table: "RequestStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
