using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "catalog_pages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    localization = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    icon = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    layout = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "'default_3x3'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_data = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    text_data = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    visible = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_pages", x => x.id);
                    table.ForeignKey(
                        name: "FK_catalog_pages_catalog_pages_parent_id",
                        column: x => x.parent_id,
                        principalTable: "catalog_pages",
                        principalColumn: "id");
                })
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
                    logic = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, defaultValueSql: "'default'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_states = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    x = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    y = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    z = table.Column<double>(type: "double(10,3)", nullable: false, defaultValueSql: "0"),
                    can_stack = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    can_walk = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    can_sit = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    can_lay = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    can_recycle = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    can_trade = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    can_group = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    can_sell = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    usage_policy = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    extra_data = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furniture_definitions", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "navigator_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    localization_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_public = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_categories", x => x.id);
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
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_event_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "navigator_tabs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    search_code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navigator_tabs", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "performance_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    elapsed_time = table.Column<int>(type: "int", nullable: false),
                    user_agent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    flash_version = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    os = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    browser = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_debugger = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    memory_usage = table.Column<int>(type: "int", nullable: false),
                    garbage_collections = table.Column<int>(type: "int", nullable: false),
                    average_frame_rate = table.Column<int>(type: "int", nullable: false),
                    ip_address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_performance_logs", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_chat_styles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_style_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_chat_styles", x => x.id);
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
                    figure = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValueSql: "'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    status = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    room_chat_style_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    custom = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_models", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "catalog_offers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    page_id = table.Column<int>(type: "int", nullable: false),
                    localization_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cost_credits = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    cost_currency = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    currency_type = table.Column<int>(type: "int", nullable: true),
                    can_gift = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    can_bundle = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    club_level = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    visible = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_offers", x => x.id);
                    table.ForeignKey(
                        name: "FK_catalog_offers_catalog_pages_page_id",
                        column: x => x.page_id,
                        principalTable: "catalog_pages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "messenger_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messenger_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_messenger_categories_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "messenger_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    requested_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messenger_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_messenger_requests_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messenger_requests_players_requested_id",
                        column: x => x.requested_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_badges",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    badge_code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    slot_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_badges", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_badges_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_chat_styles_owned",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    chat_style_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_chat_styles_owned", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_chat_styles_owned_player_chat_styles_chat_style_id",
                        column: x => x.chat_style_id,
                        principalTable: "player_chat_styles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_chat_styles_owned_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_currencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_currencies", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_currencies_players_player_id",
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
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    category_id = table.Column<int>(type: "int", nullable: true),
                    users_now = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    users_max = table.Column<int>(type: "int", nullable: false, defaultValueSql: "25"),
                    paint_wall = table.Column<double>(type: "double", nullable: false, defaultValueSql: "0"),
                    paint_floor = table.Column<double>(type: "double", nullable: false, defaultValueSql: "0"),
                    paint_landscape = table.Column<double>(type: "double", nullable: false, defaultValueSql: "0"),
                    wall_height = table.Column<int>(type: "int", nullable: false, defaultValueSql: "-1"),
                    hide_walls = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    thickness_wall = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    thickness_floor = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    allow_walk_through = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    allow_editing = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "1"),
                    allow_pets = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    allow_pets_eat = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                    trade_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    mute_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    kick_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    ban_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    chat_mode_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    chat_weight_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    chat_speed_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    chat_protection_type = table.Column<int>(type: "int", nullable: false, defaultValueSql: "2"),
                    chat_distance = table.Column<int>(type: "int", nullable: false, defaultValueSql: "50"),
                    last_active = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_navigator_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "navigator_categories",
                        principalColumn: "id");
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
                name: "catalog_products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    offer_id = table.Column<int>(type: "int", nullable: false),
                    product_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    definition_id = table.Column<int>(type: "int", nullable: true),
                    extra_param = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    quantity = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    unique_size = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    unique_remaining = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_catalog_products_catalog_offers_offer_id",
                        column: x => x.offer_id,
                        principalTable: "catalog_offers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_catalog_products_furniture_definitions_definition_id",
                        column: x => x.definition_id,
                        principalTable: "furniture_definitions",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "messenger_friends",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    requested_id = table.Column<int>(type: "int", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    relation = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messenger_friends", x => x.id);
                    table.ForeignKey(
                        name: "FK_messenger_friends_messenger_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "messenger_categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messenger_friends_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messenger_friends_players_requested_id",
                        column: x => x.requested_id,
                        principalTable: "players",
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
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    date_expires = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                name: "room_chatlogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    target_player_id = table.Column<int>(type: "int", nullable: true),
                    message = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_chatlogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_room_chatlogs_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_chatlogs_players_target_player_id",
                        column: x => x.target_player_id,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_room_chatlogs_rooms_room_id",
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
                    date_expires = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                name: "IX_catalog_offers_page_id",
                table: "catalog_offers",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_pages_parent_id",
                table: "catalog_pages",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_products_definition_id",
                table: "catalog_products",
                column: "definition_id");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_products_offer_id",
                table: "catalog_products",
                column: "offer_id");

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
                name: "IX_messenger_categories_player_id_name",
                table: "messenger_categories",
                columns: new[] { "player_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_messenger_friends_category_id",
                table: "messenger_friends",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_messenger_friends_player_id_requested_id",
                table: "messenger_friends",
                columns: new[] { "player_id", "requested_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_messenger_friends_requested_id",
                table: "messenger_friends",
                column: "requested_id");

            migrationBuilder.CreateIndex(
                name: "IX_messenger_requests_player_id_requested_id",
                table: "messenger_requests",
                columns: new[] { "player_id", "requested_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_messenger_requests_requested_id",
                table: "messenger_requests",
                column: "requested_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_performance_logs_elapsed_time",
                table: "performance_logs",
                column: "elapsed_time");

            migrationBuilder.CreateIndex(
                name: "IX_performance_logs_ip_address",
                table: "performance_logs",
                column: "ip_address");

            migrationBuilder.CreateIndex(
                name: "IX_player_badges_player_id_badge_code",
                table: "player_badges",
                columns: new[] { "player_id", "badge_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_chat_styles_client_style_id",
                table: "player_chat_styles",
                column: "client_style_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_chat_styles_owned_chat_style_id",
                table: "player_chat_styles_owned",
                column: "chat_style_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_chat_styles_owned_player_id_chat_style_id",
                table: "player_chat_styles_owned",
                columns: new[] { "player_id", "chat_style_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_currencies_player_id_type",
                table: "player_currencies",
                columns: new[] { "player_id", "type" },
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
                name: "IX_room_chatlogs_player_id",
                table: "room_chatlogs",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_chatlogs_room_id",
                table: "room_chatlogs",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_chatlogs_target_player_id",
                table: "room_chatlogs",
                column: "target_player_id");

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
                name: "IX_rooms_category_id",
                table: "rooms",
                column: "category_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalog_products");

            migrationBuilder.DropTable(
                name: "furniture_teleport_links");

            migrationBuilder.DropTable(
                name: "messenger_friends");

            migrationBuilder.DropTable(
                name: "messenger_requests");

            migrationBuilder.DropTable(
                name: "navigator_event_categories");

            migrationBuilder.DropTable(
                name: "navigator_tabs");

            migrationBuilder.DropTable(
                name: "performance_logs");

            migrationBuilder.DropTable(
                name: "player_badges");

            migrationBuilder.DropTable(
                name: "player_chat_styles_owned");

            migrationBuilder.DropTable(
                name: "player_currencies");

            migrationBuilder.DropTable(
                name: "room_bans");

            migrationBuilder.DropTable(
                name: "room_chatlogs");

            migrationBuilder.DropTable(
                name: "room_mutes");

            migrationBuilder.DropTable(
                name: "room_rights");

            migrationBuilder.DropTable(
                name: "security_tickets");

            migrationBuilder.DropTable(
                name: "catalog_offers");

            migrationBuilder.DropTable(
                name: "furniture");

            migrationBuilder.DropTable(
                name: "messenger_categories");

            migrationBuilder.DropTable(
                name: "player_chat_styles");

            migrationBuilder.DropTable(
                name: "catalog_pages");

            migrationBuilder.DropTable(
                name: "furniture_definitions");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "navigator_categories");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "room_models");
        }
    }
}
