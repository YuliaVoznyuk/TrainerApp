using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlineTrainingSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "ScheduleSlots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OnlineMeetingUrl",
                table: "ScheduleSlots",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "ScheduleSlots");

            migrationBuilder.DropColumn(
                name: "OnlineMeetingUrl",
                table: "ScheduleSlots");
        }
    }
}
