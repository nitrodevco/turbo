using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Turbo.Database.Migrations
{
    public partial class FurnitureTeleportLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "furniture_teleport_links",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    furniture_one_id = table.Column<int>(type: "int", nullable: false),
                    furniture_two_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furniture_teleport_links", x => x.id);
                    table.ForeignKey(
                        name: "FK_furniture_teleport_links_furniture_furniture_one_id",
                        column: x => x.furniture_one_id,
                        principalTable: "furniture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_furniture_teleport_links_furniture_furniture_two_id",
                        column: x => x.furniture_two_id,
                        principalTable: "furniture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_furniture_teleport_links_furniture_one_id",
                table: "furniture_teleport_links",
                column: "furniture_one_id");

            migrationBuilder.CreateIndex(
                name: "IX_furniture_teleport_links_furniture_two_id",
                table: "furniture_teleport_links",
                column: "furniture_two_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "furniture_teleport_link");
        }
    }
}
