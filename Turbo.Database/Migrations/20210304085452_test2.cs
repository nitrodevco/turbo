using Microsoft.EntityFrameworkCore.Migrations;

namespace Turbo.Database.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "room_models",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "room_models",
                newName: "model");

            migrationBuilder.RenameColumn(
                name: "Enabled",
                table: "room_models",
                newName: "enabled");

            migrationBuilder.RenameColumn(
                name: "Custom",
                table: "room_models",
                newName: "custom");

            migrationBuilder.RenameColumn(
                name: "DoorY",
                table: "room_models",
                newName: "door_y");

            migrationBuilder.RenameColumn(
                name: "DoorX",
                table: "room_models",
                newName: "door_x");

            migrationBuilder.RenameColumn(
                name: "DoorDirection",
                table: "room_models",
                newName: "door_direction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "room_models",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "model",
                table: "room_models",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "enabled",
                table: "room_models",
                newName: "Enabled");

            migrationBuilder.RenameColumn(
                name: "custom",
                table: "room_models",
                newName: "Custom");

            migrationBuilder.RenameColumn(
                name: "door_y",
                table: "room_models",
                newName: "DoorY");

            migrationBuilder.RenameColumn(
                name: "door_x",
                table: "room_models",
                newName: "DoorX");

            migrationBuilder.RenameColumn(
                name: "door_direction",
                table: "room_models",
                newName: "DoorDirection");
        }
    }
}
