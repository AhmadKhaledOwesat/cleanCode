using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcpTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class T1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestStatusId",
                table: "ServiceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestStatusId",
                table: "ServiceRequestResponses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_RequestStatusId",
                table: "ServiceRequests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestResponses_RequestStatusId",
                table: "ServiceRequestResponses",
                column: "RequestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestResponses_RequestStatus_RequestStatusId",
                table: "ServiceRequestResponses",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id");

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
                name: "FK_ServiceRequestResponses_RequestStatus_RequestStatusId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_RequestStatus_RequestStatusId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_RequestStatusId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequestResponses_RequestStatusId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropColumn(
                name: "RequestStatusId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "RequestStatusId",
                table: "ServiceRequestResponses");
        }
    }
}
