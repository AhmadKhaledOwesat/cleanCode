using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcpTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainSubServiceMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MainServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainSubServiceMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainSubServiceMappings_MainServices_MainServiceId",
                        column: x => x.MainServiceId,
                        principalTable: "MainServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MainSubServiceMappings_SubServices_SubServiceId",
                        column: x => x.SubServiceId,
                        principalTable: "SubServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    RequestBriefly = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentFileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPaid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_AppUserAddress_AddressId",
                        column: x => x.AddressId,
                        principalTable: "AppUserAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequests_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequests_SubServices_SubServiceId",
                        column: x => x.SubServiceId,
                        principalTable: "SubServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TechnicianServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicianServices_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TechnicianServices_SubServices_SubServiceId",
                        column: x => x.SubServiceId,
                        principalTable: "SubServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionNameIOS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionLinkIOS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequired = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResponseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ResponseTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResponseId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequestResponses_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequestResponses_ServiceRequests_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainSubServiceMappings_MainServiceId",
                table: "MainSubServiceMappings",
                column: "MainServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MainSubServiceMappings_SubServiceId",
                table: "MainSubServiceMappings",
                column: "SubServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestResponses_AppUserId",
                table: "ServiceRequestResponses",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestResponses_ServiceRequestId",
                table: "ServiceRequestResponses",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_AddressId",
                table: "ServiceRequests",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_AppUserId",
                table: "ServiceRequests",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_SubServiceId",
                table: "ServiceRequests",
                column: "SubServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianServices_AppUserId",
                table: "TechnicianServices",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianServices_SubServiceId",
                table: "TechnicianServices",
                column: "SubServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainSubServiceMappings");

            migrationBuilder.DropTable(
                name: "ServiceRequestResponses");

            migrationBuilder.DropTable(
                name: "TechnicianServices");

            migrationBuilder.DropTable(
                name: "Versions");

            migrationBuilder.DropTable(
                name: "ServiceRequests");
        }
    }
}
