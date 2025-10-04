using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class T : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestResponses_RequestStatus_RequestStatusId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequestResponses_RequestStatusId",
                table: "ServiceRequestResponses");

            migrationBuilder.DropColumn(
                name: "RequestStatusId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "RequestStatusId",
                table: "ServiceRequestResponses");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "RequestStatus",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "RequestStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");


            migrationBuilder.DropPrimaryKey(
        name: "PK_RequestStatus",
        table: "RequestStatus");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RequestStatus");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RequestStatus",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1"); // Adjust to previous state

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestStatus",
                table: "RequestStatus",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestStatusId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RequestStatusId",
                table: "ServiceRequestResponses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "RequestStatus",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RequestStatus",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RequestStatus",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

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
        }
    }
}
