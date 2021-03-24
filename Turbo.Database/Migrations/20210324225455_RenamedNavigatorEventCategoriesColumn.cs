using Microsoft.EntityFrameworkCore.Migrations;

namespace Turbo.Database.Migrations
{
    public partial class RenamedNavigatorEventCategoriesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_navigator_event_catagories",
                table: "navigator_event_catagories");

            migrationBuilder.RenameTable(
                name: "navigator_event_catagories",
                newName: "navigator_event_categories");

            migrationBuilder.RenameColumn(
                name: "chat_type",
                table: "rooms",
                newName: "chat_mode_type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "navigator_event_categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Enabled",
                table: "navigator_event_categories",
                newName: "enabled");

            migrationBuilder.AddPrimaryKey(
                name: "PK_navigator_event_categories",
                table: "navigator_event_categories",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_navigator_event_categories",
                table: "navigator_event_categories");

            migrationBuilder.RenameTable(
                name: "navigator_event_categories",
                newName: "navigator_event_catagories");

            migrationBuilder.RenameColumn(
                name: "chat_mode_type",
                table: "rooms",
                newName: "chat_type");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "navigator_event_catagories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "enabled",
                table: "navigator_event_catagories",
                newName: "Enabled");

            migrationBuilder.AddPrimaryKey(
                name: "PK_navigator_event_catagories",
                table: "navigator_event_catagories",
                column: "id");
        }
    }
}
