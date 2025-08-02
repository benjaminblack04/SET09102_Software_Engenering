using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class THing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_role_users_Id",
                table: "user_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_attendees",
                table: "event_attendees");

            migrationBuilder.DropIndex(
                name: "IX_event_attendees_EventId_UserId",
                table: "event_attendees");

            migrationBuilder.DropIndex(
                name: "IX_event_Id_SpeakerId",
                table: "event");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "user_role",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "event",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "event",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_attendees",
                table: "event_attendees",
                columns: new[] { "EventId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_event_attendees",
                table: "event_attendees");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "user_role",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "event",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "event",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_attendees",
                table: "event_attendees",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_event_attendees_EventId_UserId",
                table: "event_attendees",
                columns: new[] { "EventId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_event_Id_SpeakerId",
                table: "event",
                columns: new[] { "Id", "SpeakerId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_users_Id",
                table: "user_role",
                column: "Id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
