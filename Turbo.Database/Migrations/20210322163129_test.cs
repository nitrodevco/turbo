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
                    public_name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    product_name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    logic = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    total_states = table.Column<int>(type: "int", nullable: false),
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
                    extra_data = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
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
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    motto = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    figure = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    gender = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
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
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    model = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    door_x = table.Column<int>(type: "int", nullable: false),
                    door_y = table.Column<int>(type: "int", nullable: false),
                    door_rotation = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    custom = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "security_tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    ticket = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    ip_address = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
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
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    users_now = table.Column<int>(type: "int", nullable: false),
                    users_max = table.Column<int>(type: "int", nullable: false),
                    paint_wall = table.Column<double>(type: "double", nullable: false),
                    paint_floor = table.Column<double>(type: "double", nullable: false),
                    paint_landscape = table.Column<double>(type: "double", nullable: false),
                    wall_height = table.Column<int>(type: "int", nullable: false),
                    hide_walls = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    thickness_wall = table.Column<int>(type: "int", nullable: false),
                    thickness_floor = table.Column<int>(type: "int", nullable: false),
                    allow_walk_through = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    allow_editing = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    allow_pets = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    allow_pets_eat = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    trade_type = table.Column<int>(type: "int", nullable: false),
                    mute_type = table.Column<int>(type: "int", nullable: false),
                    kick_type = table.Column<int>(type: "int", nullable: false),
                    ban_type = table.Column<int>(type: "int", nullable: false),
                    chat_type = table.Column<int>(type: "int", nullable: false),
                    chat_weight_type = table.Column<int>(type: "int", nullable: false),
                    chat_speed_type = table.Column<int>(type: "int", nullable: false),
                    chat_protection_type = table.Column<int>(type: "int", nullable: false),
                    chat_distance = table.Column<int>(type: "int", nullable: false),
                    last_active = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rooms_room_models_model_id",
                        column: x => x.model_id,
                        principalTable: "room_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "furniture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    definition_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: true),
                    x = table.Column<int>(type: "int", nullable: false),
                    y = table.Column<int>(type: "int", nullable: false),
                    z = table.Column<double>(type: "double", nullable: false),
                    direction = table.Column<int>(type: "int", nullable: false),
                    wall_position = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    stuff_data = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_furniture_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_furniture_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_furniture_player_id",
                table: "furniture",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_furniture_room_id",
                table: "furniture",
                column: "room_id");

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
                name: "IX_rooms_player_id",
                table: "rooms",
                column: "player_id");

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
