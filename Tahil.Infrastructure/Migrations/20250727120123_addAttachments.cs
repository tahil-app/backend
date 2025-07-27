using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attachment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teacher_attachment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    attachment_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_attachment", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_attachment_attachment_attachment_id",
                        column: x => x.attachment_id,
                        principalTable: "attachment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_attachment_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: 1,
                column: "role",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_teacher_attachment_attachment_id",
                table: "teacher_attachment",
                column: "attachment_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_attachment_teacher_id",
                table: "teacher_attachment",
                column: "teacher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "teacher_attachment");

            migrationBuilder.DropTable(
                name: "attachment");

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: 1,
                column: "role",
                value: 0);
        }
    }
}
