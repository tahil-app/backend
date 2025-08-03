using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTenantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tenant",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    sub_domain = table.Column<string>(type: "text", nullable: true),
                    logo_url = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "tenant",
                columns: new[] { "id", "is_active", "logo_url", "name", "sub_domain" },
                values: new object[] { new Guid("6af39530-f6e0-4298-a890-fb5c50310c7c"), true, null, "دار الفرقان", null });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: 1,
                column: "tenant_id",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_user_tenant_id",
                table: "user",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_tenant_tenant_id",
                table: "user",
                column: "tenant_id",
                principalTable: "tenant",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_tenant_tenant_id",
                table: "user");

            migrationBuilder.DropTable(
                name: "tenant");

            migrationBuilder.DropIndex(
                name: "IX_user_tenant_id",
                table: "user");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "user");
        }
    }
}
