using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAdminSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "is-active", "name", "password", "phone-number", "role", "email" },
                values: new object[] { 1, true, "Admin", "$2a$11$GGEGcXpeP39kYGG6NFIS3O0EskcYWfucTDcK8Y8kBLJw93iDqcjSG", "0000", 0, "admin@be.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
