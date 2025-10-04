using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcpTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitRequestServiceRequestResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestResponses_AppUser_AppUserId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "ServiceRequestResponses");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "ServiceRequestResponses",
                newName: "RequestStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRequestResponses_AppUserId",
                table: "ServiceRequestResponses",
                newName: "IX_ServiceRequestResponses_RequestStatusId");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "ServiceRequestResponses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "ServiceRequestResponses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestResponses_ClientId",
                table: "ServiceRequestResponses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestResponses_ProviderId",
                table: "ServiceRequestResponses",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestResponses_AppUser_ClientId",
                table: "ServiceRequestResponses",
                column: "ClientId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestResponses_AppUser_ProviderId",
                table: "ServiceRequestResponses",
                column: "ProviderId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestResponses_RequestStatus_RequestStatusId",
                table: "ServiceRequestResponses",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestResponses_AppUser_ClientId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestResponses_AppUser_ProviderId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestResponses_RequestStatus_RequestStatusId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequestResponses_ClientId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequestResponses_ProviderId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "ServiceRequestResponses");

            migrationBuilder.RenameColumn(
                name: "RequestStatusId",
                table: "ServiceRequestResponses",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRequestResponses_RequestStatusId",
                table: "ServiceRequestResponses",
                newName: "IX_ServiceRequestResponses_AppUserId");

            migrationBuilder.AddColumn<int>(
                name: "ResponseId",
                table: "ServiceRequestResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestResponses_AppUser_AppUserId",
                table: "ServiceRequestResponses",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }
    }
}
