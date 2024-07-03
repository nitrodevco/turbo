using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class Navigator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "slot_id",
                table: "player_currencies",
                newName: "amount");

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "navigator_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_public = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_tabs", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_category_id",
                table: "rooms",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_navigator_categories_name",
                table: "navigator_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_navigator_tabs_name",
                table: "navigator_tabs",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_navigator_categories_category_id",
                table: "rooms",
                column: "category_id",
                principalTable: "navigator_categories",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_navigator_categories_category_id",
                table: "rooms");

            migrationBuilder.DropTable(
                name: "navigator_categories");

            migrationBuilder.DropTable(
                name: "navigator_tabs");

            migrationBuilder.DropIndex(
                name: "IX_rooms_category_id",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "rooms");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "player_currencies",
                newName: "slot_id");
        }
    }
}
