using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateClassSessionColumnNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_class_session_room_override_room_id",
                table: "class_session");

            migrationBuilder.DropForeignKey(
                name: "FK_class_session_teacher_override_teacher_id",
                table: "class_session");

            migrationBuilder.RenameColumn(
                name: "override_teacher_id",
                table: "class_session",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "override_start_time",
                table: "class_session",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "override_room_id",
                table: "class_session",
                newName: "room_id");

            migrationBuilder.RenameColumn(
                name: "override_end_time",
                table: "class_session",
                newName: "end_time");

            migrationBuilder.RenameIndex(
                name: "IX_class_session_override_teacher_id",
                table: "class_session",
                newName: "IX_class_session_teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_class_session_override_room_id",
                table: "class_session",
                newName: "IX_class_session_room_id");

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_room_room_id",
                table: "class_session",
                column: "room_id",
                principalTable: "room",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_teacher_teacher_id",
                table: "class_session",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_class_session_room_room_id",
                table: "class_session");

            migrationBuilder.DropForeignKey(
                name: "FK_class_session_teacher_teacher_id",
                table: "class_session");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "class_session",
                newName: "override_teacher_id");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "class_session",
                newName: "override_start_time");

            migrationBuilder.RenameColumn(
                name: "room_id",
                table: "class_session",
                newName: "override_room_id");

            migrationBuilder.RenameColumn(
                name: "end_time",
                table: "class_session",
                newName: "override_end_time");

            migrationBuilder.RenameIndex(
                name: "IX_class_session_teacher_id",
                table: "class_session",
                newName: "IX_class_session_override_teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_class_session_room_id",
                table: "class_session",
                newName: "IX_class_session_override_room_id");

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_room_override_room_id",
                table: "class_session",
                column: "override_room_id",
                principalTable: "room",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_teacher_override_teacher_id",
                table: "class_session",
                column: "override_teacher_id",
                principalTable: "teacher",
                principalColumn: "id");
        }
    }
}
