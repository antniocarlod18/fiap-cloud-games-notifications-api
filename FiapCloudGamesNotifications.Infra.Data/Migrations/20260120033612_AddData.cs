using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGamesNotifications.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserNotificationProfile",
                columns: new[] { "Id", "UserId", "DateCreated", "Active", "Email" },
                values: new object[] { new Guid("1f511254-b952-4569-aa72-6a3527a5971f"), new Guid("bbbbbbbb-2222-3333-4444-555555555555"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "admin@local" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
