using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationApkMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "Applications",
                type: "NVARCHAR(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersionName",
                table: "Applications",
                type: "NVARCHAR(50)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AppSize",
                table: "Applications",
                type: "BIGINT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "VersionName",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AppSize",
                table: "Applications");
        }
    }
}
