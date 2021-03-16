using Microsoft.EntityFrameworkCore.Migrations;

namespace Turbo.Database.Migrations
{
    public partial class updatefurniture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowWalkThrough",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "figure",
                table: "players",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "players",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "extra_data",
                table: "furniture_definitions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AddColumn<int>(
                name: "total_states",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowWalkThrough",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "figure",
                table: "players");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "players");

            migrationBuilder.DropColumn(
                name: "total_states",
                table: "furniture_definitions");

            migrationBuilder.AlterColumn<bool>(
                name: "extra_data",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);
        }
    }
}
