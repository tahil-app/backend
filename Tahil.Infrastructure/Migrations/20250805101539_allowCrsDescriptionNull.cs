using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class allowCrsDescriptionNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "course",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "attachment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_attachment_TenantId",
                table: "attachment",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_attachment_tenant_TenantId",
                table: "attachment",
                column: "TenantId",
                principalTable: "tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attachment_tenant_TenantId",
                table: "attachment");

            migrationBuilder.DropIndex(
                name: "IX_attachment_TenantId",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "attachment");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "course",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
