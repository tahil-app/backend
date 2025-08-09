using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addClassSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_session_lesson_schedule_schedule_id",
                table: "lesson_session");

            migrationBuilder.DropTable(
                name: "lesson_schedule");

            migrationBuilder.CreateTable(
                name: "class_schedule",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false),
                    day = table.Column<int>(type: "integer", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: false),
                    ClassScheduleId = table.Column<int>(type: "integer", nullable: true),
                    CourseId = table.Column<int>(type: "integer", nullable: true),
                    TeacherId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_schedule", x => x.id);
                    table.ForeignKey(
                        name: "FK_class_schedule_class_schedule_ClassScheduleId",
                        column: x => x.ClassScheduleId,
                        principalTable: "class_schedule",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_class_schedule_course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_class_schedule_group_group_id",
                        column: x => x.group_id,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_schedule_room_room_id",
                        column: x => x.room_id,
                        principalTable: "room",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_schedule_teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "teacher",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_class_schedule_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_ClassScheduleId",
                table: "class_schedule",
                column: "ClassScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_CourseId",
                table: "class_schedule",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_group_id",
                table: "class_schedule",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_room_id",
                table: "class_schedule",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_TeacherId",
                table: "class_schedule",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_tenant_id",
                table: "class_schedule",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_session_class_schedule_schedule_id",
                table: "lesson_session",
                column: "schedule_id",
                principalTable: "class_schedule",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_session_class_schedule_schedule_id",
                table: "lesson_session");

            migrationBuilder.DropTable(
                name: "class_schedule");

            migrationBuilder.CreateTable(
                name: "lesson_schedule",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false),
                    reference = table.Column<int>(type: "integer", nullable: true),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    day = table.Column<int>(type: "integer", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_schedule", x => x.id);
                    table.ForeignKey(
                        name: "FK_lesson_schedule_course_course_id",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lesson_schedule_group_group_id",
                        column: x => x.group_id,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lesson_schedule_lesson_schedule_reference",
                        column: x => x.reference,
                        principalTable: "lesson_schedule",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_lesson_schedule_room_room_id",
                        column: x => x.room_id,
                        principalTable: "room",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lesson_schedule_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_course_id",
                table: "lesson_schedule",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_group_id",
                table: "lesson_schedule",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_reference",
                table: "lesson_schedule",
                column: "reference");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_room_id",
                table: "lesson_schedule",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_teacher_id",
                table: "lesson_schedule",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_session_lesson_schedule_schedule_id",
                table: "lesson_session",
                column: "schedule_id",
                principalTable: "lesson_schedule",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
