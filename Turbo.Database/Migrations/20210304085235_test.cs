using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

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
                name: "room_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DoorX = table.Column<int>(type: "int", nullable: false),
                    DoorY = table.Column<int>(type: "int", nullable: false),
                    DoorDirection = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Custom = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "furniture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    definition_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furniture", x => x.id);
                    table.ForeignKey(
                        name: "FK_furniture_furniture_definitions_definition_id",
                        column: x => x.definition_id,
                        principalTable: "furniture_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "security_tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_security_tickets_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_room_models_model_id",
                        column: x => x.model_id,
                        principalTable: "room_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_bans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_bans", x => x.id);
                    table.ForeignKey(
                        name: "FK_room_bans_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_bans_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_mutes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_mutes", x => x.id);
                    table.ForeignKey(
                        name: "FK_room_mutes_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_mutes_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_rights",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_rights", x => x.id);
                    table.ForeignKey(
                        name: "FK_room_rights_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_rights_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_furniture_definition_id",
                table: "furniture",
                column: "definition_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_bans_player_id",
                table: "room_bans",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_bans_room_id",
                table: "room_bans",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_mutes_player_id",
                table: "room_mutes",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_mutes_room_id",
                table: "room_mutes",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_rights_player_id",
                table: "room_rights",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_rights_room_id",
                table: "room_rights",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_model_id",
                table: "rooms",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_security_tickets_player_id",
                table: "security_tickets",
                column: "player_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "furniture");

            migrationBuilder.DropTable(
                name: "room_bans");

            migrationBuilder.DropTable(
                name: "room_mutes");

            migrationBuilder.DropTable(
                name: "room_rights");

            migrationBuilder.DropTable(
                name: "security_tickets");

            migrationBuilder.DropTable(
                name: "furniture_definitions");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "room_models");
        }
    }
}
