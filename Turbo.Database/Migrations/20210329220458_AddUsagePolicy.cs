using Microsoft.EntityFrameworkCore.Migrations;

namespace Turbo.Database.Migrations
{
    public partial class AddUsagePolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "usage_policy",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usage_policy",
                table: "furniture_definitions");
        }
    }
}
