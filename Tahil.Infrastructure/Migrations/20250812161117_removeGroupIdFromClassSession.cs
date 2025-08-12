using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeGroupIdFromClassSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_class_session_group_GroupId",
                table: "class_session");

            migrationBuilder.DropIndex(
                name: "IX_class_session_GroupId",
                table: "class_session");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "class_session");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "class_session",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_class_session_GroupId",
                table: "class_session",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_class_session_group_GroupId",
                table: "class_session",
                column: "GroupId",
                principalTable: "group",
                principalColumn: "id");
        }
    }
}
