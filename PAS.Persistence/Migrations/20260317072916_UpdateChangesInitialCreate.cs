using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChangesInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_AspNetUsers_AspNetUserId",
                        column: x => x.AspNetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLogins_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLogins_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ApprovedById",
                table: "ServiceRequests",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_RequesterId",
                table: "ServiceRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingNotes_ReceivedById",
                table: "ReceivingNotes",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionLogs_InspectorId",
                table: "InspectionLogs",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_UserId",
                table: "AuditTrails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvers_UserId",
                table: "Approvers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_AspNetUserId",
                table: "UserLogins",
                column: "AspNetUserId",
                unique: true,
                filter: "[AspNetUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_EmployeeId",
                table: "UserLogins",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_RoleId",
                table: "UserLogins",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_Username",
                table: "UserLogins",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvers_UserLogins_UserId",
                table: "Approvers",
                column: "UserId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrails_UserLogins_UserId",
                table: "AuditTrails",
                column: "UserId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionLogs_UserLogins_InspectorId",
                table: "InspectionLogs",
                column: "InspectorId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserLogins_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivingNotes_UserLogins_ReceivedById",
                table: "ReceivingNotes",
                column: "ReceivedById",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_UserLogins_ApprovedById",
                table: "ServiceRequests",
                column: "ApprovedById",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_UserLogins_RequesterId",
                table: "ServiceRequests",
                column: "RequesterId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvers_UserLogins_UserId",
                table: "Approvers");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_UserLogins_UserId",
                table: "AuditTrails");

            migrationBuilder.DropForeignKey(
                name: "FK_InspectionLogs_UserLogins_InspectorId",
                table: "InspectionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserLogins_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceivingNotes_UserLogins_ReceivedById",
                table: "ReceivingNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_UserLogins_ApprovedById",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_UserLogins_RequesterId",
                table: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_ApprovedById",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_RequesterId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReceivingNotes_ReceivedById",
                table: "ReceivingNotes");

            migrationBuilder.DropIndex(
                name: "IX_InspectionLogs_InspectorId",
                table: "InspectionLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_UserId",
                table: "AuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_Approvers_UserId",
                table: "Approvers");
        }
    }
}
