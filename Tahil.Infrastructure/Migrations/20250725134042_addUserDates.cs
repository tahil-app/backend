using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tahil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUserDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone-number",
                table: "user",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "is-active",
                table: "user",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "is-active",
                table: "room",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "is-active",
                table: "course",
                newName: "is_active");

            migrationBuilder.AddColumn<DateOnly>(
                name: "birth_date",
                table: "user",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "gender",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "joined_date",
                table: "user",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "birth_date", "gender", "joined_date" },
                values: new object[] { new DateOnly(1, 1, 1), 0, new DateOnly(1, 1, 1) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birth_date",
                table: "user");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "user");

            migrationBuilder.DropColumn(
                name: "joined_date",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "user",
                newName: "phone-number");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "user",
                newName: "is-active");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "room",
                newName: "is-active");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "course",
                newName: "is-active");
        }
    }
}
