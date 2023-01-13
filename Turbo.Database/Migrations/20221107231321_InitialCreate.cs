﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "furniture_definitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sprite_id = table.Column<int>(type: "int", nullable: false),
                    public_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    product_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "varchar(255)", nullable: false, defaultValueSql: "'s'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    logic = table.Column<string>(type: "longtext", nullable: false, defaultValueSql: "'default'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_states = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    x = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    y = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    z = table.Column<double>(type: "double(10,3)", nullable: false, defaultValueSql: "0"),
                    can_stack = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "1"),
                    can_walk = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    can_sit = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    can_lay = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    can_recycle = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    can_trade = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "1"),
                    can_group = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "1"),
                    can_sell = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "1"),
                    usage_policy = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    extra_data = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furniture_definitions", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "navigator_event_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_event_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    motto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    figure = table.Column<string>(type: "longtext", nullable: false, defaultValueSql: "'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    door_x = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    door_y = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    door_rotation = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "1"),
                    custom = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_models", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_settings_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "security_tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    ticket = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ip_address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_locked = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    users_now = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    users_max = table.Column<int>(type: "int", nullable: false, defaultValueSql: "25"),
                    paint_wall = table.Column<double>(type: "double", nullable: false, defaultValueSql: "0"),
                    paint_floor = table.Column<double>(type: "double", nullable: false, defaultValueSql: "0"),
                    paint_landscape = table.Column<double>(type: "double", nullable: false, defaultValueSql: "0"),
                    wall_height = table.Column<int>(type: "int", nullable: false, defaultValueSql: "-1"),
                    hide_walls = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    thickness_wall = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    thickness_floor = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    allow_walk_through = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "1"),
                    allow_editing = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "1"),
                    allow_pets = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    allow_pets_eat = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                    trade_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    mute_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    kick_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    ban_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    chat_mode_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    chat_weight_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    chat_speed_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    chat_protection_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "2"),
                    chat_distance = table.Column<int>(type: "int", nullable: false, defaultValueSql: "50"),
                    last_active = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "NOW()"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "furniture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    definition_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: true),
                    x = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    y = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    z = table.Column<double>(type: "double(10,3)", nullable: false, defaultValueSql: "0"),
                    direction = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    wall_position = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stuff_data = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wired_data = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room_bans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room_mutes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room_rights",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "furniture_teleport_links",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    furniture_one_id = table.Column<int>(type: "int", nullable: false),
                    furniture_two_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "IX_furniture_definitions_sprite_id_type",
                table: "furniture_definitions",
                columns: new[] { "sprite_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_furniture_teleport_links_furniture_one_id",
                table: "furniture_teleport_links",
                column: "furniture_one_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_furniture_teleport_links_furniture_two_id",
                table: "furniture_teleport_links",
                column: "furniture_two_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_settings_player_id",
                table: "player_settings",
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_players_name",
                table: "players",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_bans_player_id",
                table: "room_bans",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_bans_room_id_player_id",
                table: "room_bans",
                columns: new[] { "room_id", "player_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_models_name",
                table: "room_models",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_mutes_player_id",
                table: "room_mutes",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_mutes_room_id_player_id",
                table: "room_mutes",
                columns: new[] { "room_id", "player_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_rights_player_id",
                table: "room_rights",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_rights_room_id_player_id",
                table: "room_rights",
                columns: new[] { "room_id", "player_id" },
                unique: true);

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
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_security_tickets_ticket",
                table: "security_tickets",
                column: "ticket",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "furniture_teleport_links");

            migrationBuilder.DropTable(
                name: "navigator_event_categories");

            migrationBuilder.DropTable(
                name: "player_settings");

            migrationBuilder.DropTable(
                name: "room_bans");

            migrationBuilder.DropTable(
                name: "room_mutes");

            migrationBuilder.DropTable(
                name: "room_rights");

            migrationBuilder.DropTable(
                name: "security_tickets");

            migrationBuilder.DropTable(
                name: "furniture");

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