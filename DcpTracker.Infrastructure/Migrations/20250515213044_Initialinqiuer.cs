using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initialinqiuer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationilityId",
                table: "Inquiry");

            migrationBuilder.RenameColumn(
                name: "NationilityId",
                table: "AppUser",
                newName: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_AppUserStatusId",
                table: "AppUser",
                column: "AppUserStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_CityId",
                table: "AppUser",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_CountryId",
                table: "AppUser",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_GovernorateId",
                table: "AppUser",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_AppUserStatus_AppUserStatusId",
                table: "AppUser",
                column: "AppUserStatusId",
                principalTable: "AppUserStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_City_CityId",
                table: "AppUser",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Countries_CountryId",
                table: "AppUser",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Governorates_GovernorateId",
                table: "AppUser",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_AppUserStatus_AppUserStatusId",
                table: "AppUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_City_CityId",
                table: "AppUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_Countries_CountryId",
                table: "AppUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_Governorates_GovernorateId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_AppUserStatusId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_CityId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_CountryId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_GovernorateId",
                table: "AppUser");

            migrationBuilder.RenameColumn(
                name: "GovernorateId",
                table: "AppUser",
                newName: "NationilityId");

            migrationBuilder.AddColumn<Guid>(
                name: "NationilityId",
                table: "Inquiry",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
