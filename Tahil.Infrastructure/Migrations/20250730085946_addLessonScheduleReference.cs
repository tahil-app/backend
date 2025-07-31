using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addLessonScheduleReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "reference",
                table: "lesson_schedule",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_schedule_lesson_schedule_group_id",
                table: "lesson_schedule",
                column: "group_id",
                principalTable: "lesson_schedule",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_schedule_lesson_schedule_group_id",
                table: "lesson_schedule");

            migrationBuilder.DropColumn(
                name: "reference",
                table: "lesson_schedule");
        }
    }
}
