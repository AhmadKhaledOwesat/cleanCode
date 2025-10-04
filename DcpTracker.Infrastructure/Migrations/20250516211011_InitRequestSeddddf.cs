using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobCentra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitRequestSeddddf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "MainSliders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MainSliders");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "MainSliders");

            migrationBuilder.DropColumn(
                name: "SalesUserId",
                table: "MainSliders");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "MainSliders");

            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "MainSliders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "MainSliders");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "MainSliders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MainSliders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "MainSliders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesUserId",
                table: "MainSliders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "MainSliders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
