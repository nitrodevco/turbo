using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNavigatorEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_navigator_categories_category_id",
                table: "rooms");

            migrationBuilder.DropTable(
                name: "navigator_categories");

            migrationBuilder.DropTable(
                name: "navigator_tabs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_navigator_event_categories",
                table: "navigator_event_categories");

            migrationBuilder.RenameTable(
                name: "navigator_event_categories",
                newName: "navigator_eventcats");

            migrationBuilder.RenameColumn(
                name: "enabled",
                table: "navigator_eventcats",
                newName: "visible");

            migrationBuilder.AddPrimaryKey(
                name: "PK_navigator_eventcats",
                table: "navigator_eventcats",
                column: "id");

            migrationBuilder.CreateTable(
                name: "navigator_flatcats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    visible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    automatic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    automatic_category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    global_category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    staff_only = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    min_rank = table.Column<int>(type: "int", nullable: false),
                    order_num = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_flatcats", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "navigator_top_level_contexts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    search_code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    visible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    order_num = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_top_level_contexts", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_navigator_top_level_contexts_search_code",
                table: "navigator_top_level_contexts",
                column: "search_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_navigator_flatcats_category_id",
                table: "rooms",
                column: "category_id",
                principalTable: "navigator_flatcats",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_navigator_flatcats_category_id",
                table: "rooms");

            migrationBuilder.DropTable(
                name: "navigator_flatcats");

            migrationBuilder.DropTable(
                name: "navigator_top_level_contexts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_navigator_eventcats",
                table: "navigator_eventcats");

            migrationBuilder.RenameTable(
                name: "navigator_eventcats",
                newName: "navigator_event_categories");

            migrationBuilder.RenameColumn(
                name: "visible",
                table: "navigator_event_categories",
                newName: "enabled");

            migrationBuilder.AddPrimaryKey(
                name: "PK_navigator_event_categories",
                table: "navigator_event_categories",
                column: "id");

            migrationBuilder.CreateTable(
                name: "navigator_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_public = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    localization_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "navigator_tabs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    search_code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_tabs", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_navigator_categories_name",
                table: "navigator_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_navigator_tabs_search_code",
                table: "navigator_tabs",
                column: "search_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_navigator_categories_category_id",
                table: "rooms",
                column: "category_id",
                principalTable: "navigator_categories",
                principalColumn: "id");
        }
    }
}
