using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateClassScheduleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_schedule_lesson_schedule_group_id",
                table: "lesson_schedule");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_reference",
                table: "lesson_schedule",
                column: "reference");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_schedule_lesson_schedule_reference",
                table: "lesson_schedule",
                column: "reference",
                principalTable: "lesson_schedule",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_schedule_lesson_schedule_reference",
                table: "lesson_schedule");

            migrationBuilder.DropIndex(
                name: "IX_lesson_schedule_reference",
                table: "lesson_schedule");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_schedule_lesson_schedule_group_id",
                table: "lesson_schedule",
                column: "group_id",
                principalTable: "lesson_schedule",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
