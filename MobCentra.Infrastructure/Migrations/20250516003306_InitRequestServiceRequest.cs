using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitRequestServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianServices_AppUser_AppUserId",
                table: "TechnicianServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianServices_SubServices_SubServiceId",
                table: "TechnicianServices");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "TechnicianServices");

            migrationBuilder.RenameColumn(
                name: "SubServiceId",
                table: "TechnicianServices",
                newName: "ProviderId");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "TechnicianServices",
                newName: "MainServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianServices_SubServiceId",
                table: "TechnicianServices",
                newName: "IX_TechnicianServices_ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianServices_AppUserId",
                table: "TechnicianServices",
                newName: "IX_TechnicianServices_MainServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianServices_AppUser_ProviderId",
                table: "TechnicianServices",
                column: "ProviderId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianServices_MainServices_MainServiceId",
                table: "TechnicianServices",
                column: "MainServiceId",
                principalTable: "MainServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianServices_AppUser_ProviderId",
                table: "TechnicianServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianServices_MainServices_MainServiceId",
                table: "TechnicianServices");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "TechnicianServices",
                newName: "SubServiceId");

            migrationBuilder.RenameColumn(
                name: "MainServiceId",
                table: "TechnicianServices",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianServices_ProviderId",
                table: "TechnicianServices",
                newName: "IX_TechnicianServices_SubServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianServices_MainServiceId",
                table: "TechnicianServices",
                newName: "IX_TechnicianServices_AppUserId");

            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "TechnicianServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianServices_AppUser_AppUserId",
                table: "TechnicianServices",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianServices_SubServices_SubServiceId",
                table: "TechnicianServices",
                column: "SubServiceId",
                principalTable: "SubServices",
                principalColumn: "Id");
        }
    }
}
