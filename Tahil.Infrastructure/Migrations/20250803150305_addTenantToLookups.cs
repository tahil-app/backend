using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTenantToLookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "capacity",
                table: "group",
                newName: "teacher_id");

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "room",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "course_id",
                table: "group",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "group",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "course",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "course",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_room_tenant_id",
                table: "room",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_course_id",
                table: "group",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_teacher_id",
                table: "group",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_tenant_id",
                table: "group",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_tenant_id",
                table: "course",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_course_tenant_tenant_id",
                table: "course",
                column: "tenant_id",
                principalTable: "tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_course_course_id",
                table: "group",
                column: "course_id",
                principalTable: "course",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_teacher_teacher_id",
                table: "group",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_tenant_tenant_id",
                table: "group",
                column: "tenant_id",
                principalTable: "tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_room_tenant_tenant_id",
                table: "room",
                column: "tenant_id",
                principalTable: "tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_course_tenant_tenant_id",
                table: "course");

            migrationBuilder.DropForeignKey(
                name: "FK_group_course_course_id",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_group_teacher_teacher_id",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_group_tenant_tenant_id",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_room_tenant_tenant_id",
                table: "room");

            migrationBuilder.DropIndex(
                name: "IX_room_tenant_id",
                table: "room");

            migrationBuilder.DropIndex(
                name: "IX_group_course_id",
                table: "group");

            migrationBuilder.DropIndex(
                name: "IX_group_teacher_id",
                table: "group");

            migrationBuilder.DropIndex(
                name: "IX_group_tenant_id",
                table: "group");

            migrationBuilder.DropIndex(
                name: "IX_course_tenant_id",
                table: "course");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "room");

            migrationBuilder.DropColumn(
                name: "course_id",
                table: "group");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "group");

            migrationBuilder.DropColumn(
                name: "description",
                table: "course");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "course");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "group",
                newName: "capacity");
        }
    }
}
