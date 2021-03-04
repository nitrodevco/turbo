using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Turbo.Database.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "furniture_definitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sprite_id = table.Column<int>(type: "int", nullable: false),
                    public_name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    product_name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    logic = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    x = table.Column<int>(type: "int", nullable: false),
                    y = table.Column<int>(type: "int", nullable: false),
                    z = table.Column<double>(type: "double", nullable: false),
                    can_stack = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_walk = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_sit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_lay = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_recycle = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_trade = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_group = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_sell = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    extra_data = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furniture_definitions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    motto = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "furniture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FurnitureDefinitionEntityId = table.Column<int>(type: "int", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furniture", x => x.id);
                    table.ForeignKey(
                        name: "FK_furniture_furniture_definitions_FurnitureDefinitionEntityId",
                        column: x => x.FurnitureDefinitionEntityId,
                        principalTable: "furniture_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "security_tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerEntityId = table.Column<int>(type: "int", nullable: true),
                    ticket = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ip_address = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    is_locked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_security_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_security_tickets_players_PlayerEntityId",
                        column: x => x.PlayerEntityId,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_furniture_FurnitureDefinitionEntityId",
                table: "furniture",
                column: "FurnitureDefinitionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_security_tickets_PlayerEntityId",
                table: "security_tickets",
                column: "PlayerEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "furniture");

            migrationBuilder.DropTable(
                name: "security_tickets");

            migrationBuilder.DropTable(
                name: "furniture_definitions");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
