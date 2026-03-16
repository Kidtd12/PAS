using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityUserLinkAndAuditTrailFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AspNetUserId",
                table: "UserLogins",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_AspNetUserId",
                table: "UserLogins",
                column: "AspNetUserId",
                unique: true,
                filter: "[AspNetUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_AspNetUsers_AspNetUserId",
                table: "UserLogins",
                column: "AspNetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_AspNetUsers_AspNetUserId",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_AspNetUserId",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "AspNetUserId",
                table: "UserLogins");
        }
    }
}
