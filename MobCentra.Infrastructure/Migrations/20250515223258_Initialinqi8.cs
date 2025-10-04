using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initialinqi8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GovernorateId",
                table: "Inquiry",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inquiry_AppUserStatusId",
                table: "Inquiry",
                column: "AppUserStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiry_CityId",
                table: "Inquiry",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiry_CountryId",
                table: "Inquiry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiry_GovernorateId",
                table: "Inquiry",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiry_AppUserStatus_AppUserStatusId",
                table: "Inquiry",
                column: "AppUserStatusId",
                principalTable: "AppUserStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiry_City_CityId",
                table: "Inquiry",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiry_Countries_CountryId",
                table: "Inquiry",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiry_Governorates_GovernorateId",
                table: "Inquiry",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inquiry_AppUserStatus_AppUserStatusId",
                table: "Inquiry");

            migrationBuilder.DropForeignKey(
                name: "FK_Inquiry_City_CityId",
                table: "Inquiry");

            migrationBuilder.DropForeignKey(
                name: "FK_Inquiry_Countries_CountryId",
                table: "Inquiry");

            migrationBuilder.DropForeignKey(
                name: "FK_Inquiry_Governorates_GovernorateId",
                table: "Inquiry");

            migrationBuilder.DropIndex(
                name: "IX_Inquiry_AppUserStatusId",
                table: "Inquiry");

            migrationBuilder.DropIndex(
                name: "IX_Inquiry_CityId",
                table: "Inquiry");

            migrationBuilder.DropIndex(
                name: "IX_Inquiry_CountryId",
                table: "Inquiry");

            migrationBuilder.DropIndex(
                name: "IX_Inquiry_GovernorateId",
                table: "Inquiry");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Inquiry");
        }
    }
}
