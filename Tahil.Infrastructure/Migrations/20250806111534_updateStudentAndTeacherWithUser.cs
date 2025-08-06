using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateStudentAndTeacherWithUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_teacher_user_id",
                table: "teacher");

            migrationBuilder.DropIndex(
                name: "IX_student_user_id",
                table: "student");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_user_id",
                table: "teacher",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_user_id",
                table: "student",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_teacher_user_id",
                table: "teacher");

            migrationBuilder.DropIndex(
                name: "IX_student_user_id",
                table: "student");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_user_id",
                table: "teacher",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_user_id",
                table: "student",
                column: "user_id");
        }
    }
}
