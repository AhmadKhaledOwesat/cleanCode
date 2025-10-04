using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcpTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initialinqi4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_AppUser_AppUserId",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "ServiceRequests",
                newName: "ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRequests_AppUserId",
                table: "ServiceRequests",
                newName: "IX_ServiceRequests_ProviderId");

            migrationBuilder.AlterColumn<int>(
                name: "IsPaid",
                table: "ServiceRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ClientId",
                table: "ServiceRequests",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_AppUser_ClientId",
                table: "ServiceRequests",
                column: "ClientId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_AppUser_ProviderId",
                table: "ServiceRequests",
                column: "ProviderId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_AppUser_ClientId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_AppUser_ProviderId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_ClientId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "ServiceRequests",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRequests_ProviderId",
                table: "ServiceRequests",
                newName: "IX_ServiceRequests_AppUserId");

            migrationBuilder.AlterColumn<string>(
                name: "IsPaid",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_AppUser_AppUserId",
                table: "ServiceRequests",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }
    }
}
