using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addClassSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_session_course_course_id",
                table: "lesson_session");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_session_group_group_id",
                table: "lesson_session");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_session_room_room_id",
                table: "lesson_session");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_session_teacher_teacher_id",
                table: "lesson_session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lesson_session",
                table: "lesson_session");

            migrationBuilder.DropIndex(
                name: "IX_lesson_session_course_id",
                table: "lesson_session");

            migrationBuilder.DropIndex(
                name: "IX_lesson_session_room_id",
                table: "lesson_session");

            migrationBuilder.DropIndex(
                name: "IX_lesson_session_teacher_id",
                table: "lesson_session");

            migrationBuilder.DropColumn(
                name: "course_id",
                table: "lesson_session");

            migrationBuilder.DropColumn(
                name: "note",
                table: "lesson_session");

            migrationBuilder.DropColumn(
                name: "room_id",
                table: "lesson_session");

            migrationBuilder.DropColumn(
                name: "teacher_id",
                table: "lesson_session");

            migrationBuilder.RenameTable(
                name: "lesson_session",
                newName: "class_session");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "class_session",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_lesson_session_group_id",
                table: "class_session",
                newName: "IX_class_session_GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "class_session",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "override_end_time",
                table: "class_session",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "override_room_id",
                table: "class_session",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "override_start_time",
                table: "class_session",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "override_teacher_id",
                table: "class_session",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "class_session",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_class_session",
                table: "class_session",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_class_session_override_room_id",
                table: "class_session",
                column: "override_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_session_override_teacher_id",
                table: "class_session",
                column: "override_teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_session_schedule_id",
                table: "class_session",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_session_tenant_id",
                table: "class_session",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_class_schedule_schedule_id",
                table: "class_session",
                column: "schedule_id",
                principalTable: "class_schedule",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_group_GroupId",
                table: "class_session",
                column: "GroupId",
                principalTable: "group",
                principalColumn: "id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_tenant_tenant_id",
                table: "class_session",
                column: "tenant_id",
                principalTable: "tenant",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_class_session_class_schedule_schedule_id",
                table: "class_session");

            migrationBuilder.DropForeignKey(
                name: "FK_class_session_group_GroupId",
                table: "class_session");

            migrationBuilder.DropForeignKey(
                name: "FK_class_session_room_override_room_id",
                table: "class_session");

            migrationBuilder.DropForeignKey(
                name: "FK_class_session_teacher_override_teacher_id",
                table: "class_session");

            migrationBuilder.DropForeignKey(
                name: "FK_class_session_tenant_tenant_id",
                table: "class_session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_class_session",
                table: "class_session");

            migrationBuilder.DropIndex(
                name: "IX_class_session_override_room_id",
                table: "class_session");

            migrationBuilder.DropIndex(
                name: "IX_class_session_override_teacher_id",
                table: "class_session");

            migrationBuilder.DropIndex(
                name: "IX_class_session_schedule_id",
                table: "class_session");

            migrationBuilder.DropIndex(
                name: "IX_class_session_tenant_id",
                table: "class_session");

            migrationBuilder.DropColumn(
                name: "override_end_time",
                table: "class_session");

            migrationBuilder.DropColumn(
                name: "override_room_id",
                table: "class_session");

            migrationBuilder.DropColumn(
                name: "override_start_time",
                table: "class_session");

            migrationBuilder.DropColumn(
                name: "override_teacher_id",
                table: "class_session");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "class_session");

            migrationBuilder.RenameTable(
                name: "class_session",
                newName: "lesson_session");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "lesson_session",
                newName: "group_id");

            migrationBuilder.RenameIndex(
                name: "IX_class_session_GroupId",
                table: "lesson_session",
                newName: "IX_lesson_session_group_id");

            migrationBuilder.AlterColumn<int>(
                name: "group_id",
                table: "lesson_session",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "course_id",
                table: "lesson_session",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "lesson_session",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "room_id",
                table: "lesson_session",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "teacher_id",
                table: "lesson_session",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_lesson_session",
                table: "lesson_session",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_session_course_id",
                table: "lesson_session",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_session_room_id",
                table: "lesson_session",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_session_teacher_id",
                table: "lesson_session",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_session_course_course_id",
                table: "lesson_session",
                column: "course_id",
                principalTable: "course",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_session_group_group_id",
                table: "lesson_session",
                column: "group_id",
                principalTable: "group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_session_room_room_id",
                table: "lesson_session",
                column: "room_id",
                principalTable: "room",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_session_teacher_teacher_id",
                table: "lesson_session",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
