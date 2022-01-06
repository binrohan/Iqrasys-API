using Microsoft.EntityFrameworkCore.Migrations;

namespace iqrasys.api.Migrations
{
    public partial class AddIsTrashedFieldInSolutionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTrashed",
                table: "Solutions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrashed",
                table: "Solutions");
        }
    }
}
