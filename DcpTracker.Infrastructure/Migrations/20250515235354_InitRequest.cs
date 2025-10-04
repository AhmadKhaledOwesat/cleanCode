using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcpTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestStatusId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_RequestStatusId",
                table: "ServiceRequests",
                column: "RequestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_RequestStatus_RequestStatusId",
                table: "ServiceRequests",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_RequestStatus_RequestStatusId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_RequestStatusId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "RequestStatusId",
                table: "ServiceRequests");
        }
    }
}
