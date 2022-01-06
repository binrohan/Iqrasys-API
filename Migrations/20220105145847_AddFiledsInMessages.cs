using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iqrasys.api.Migrations
{
    public partial class AddFiledsInMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTrashed",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrashed",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Messages");
        }
    }
}
