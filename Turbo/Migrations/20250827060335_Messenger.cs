using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class Messenger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_perks");

            migrationBuilder.AlterColumn<bool>(
                name: "is_locked",
                table: "security_tickets",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "wall_height",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "-1");

            migrationBuilder.AlterColumn<int>(
                name: "users_now",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "users_max",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "25");

            migrationBuilder.AlterColumn<int>(
                name: "trade_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "thickness_wall",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "thickness_floor",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "state",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "paint_wall",
                table: "rooms",
                type: "double",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "paint_landscape",
                table: "rooms",
                type: "double",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "paint_floor",
                table: "rooms",
                type: "double",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "mute_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "kick_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "hide_walls",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "chat_weight_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<int>(
                name: "chat_speed_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<int>(
                name: "chat_protection_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "2");

            migrationBuilder.AlterColumn<int>(
                name: "chat_mode_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "chat_distance",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "50");

            migrationBuilder.AlterColumn<int>(
                name: "ban_type",
                table: "rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_walk_through",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_pets_eat",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_pets",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_editing",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<bool>(
                name: "enabled",
                table: "room_models",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<int>(
                name: "door_y",
                table: "room_models",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "door_x",
                table: "room_models",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "door_rotation",
                table: "room_models",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "custom",
                table: "room_models",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "room_chatlogs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "players",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "gender",
                table: "players",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "figure",
                table: "players",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldDefaultValueSql: "'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "perk_flags",
                table: "players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "amount",
                table: "player_currencies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "global_category",
                table: "navigator_flatcats",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "automatic_category",
                table: "navigator_flatcats",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "z",
                table: "furniture_definitions",
                type: "double(10,3)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double(10,3)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "y",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<int>(
                name: "x",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "usage_policy",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "furniture_definitions",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldDefaultValueSql: "'s'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "total_states",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "logic",
                table: "furniture_definitions",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "'default'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "can_walk",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "can_trade",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<bool>(
                name: "can_stack",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<bool>(
                name: "can_sit",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "can_sell",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<bool>(
                name: "can_recycle",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "can_lay",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "can_group",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<double>(
                name: "z",
                table: "furniture",
                type: "double(10,3)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double(10,3)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "y",
                table: "furniture",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "x",
                table: "furniture",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "direction",
                table: "furniture",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "unique_size",
                table: "catalog_products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "unique_remaining",
                table: "catalog_products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "catalog_products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<bool>(
                name: "visible",
                table: "catalog_pages",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<string>(
                name: "text_data",
                table: "catalog_pages",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "layout",
                table: "catalog_pages",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldDefaultValueSql: "'default_3x3'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "image_data",
                table: "catalog_pages",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "icon",
                table: "catalog_pages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "visible",
                table: "catalog_offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<int>(
                name: "cost_currency",
                table: "catalog_offers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "cost_credits",
                table: "catalog_offers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "club_level",
                table: "catalog_offers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "can_gift",
                table: "catalog_offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<bool>(
                name: "can_bundle",
                table: "catalog_offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValueSql: "1");

            migrationBuilder.CreateTable(
                name: "logs_chat_console",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sender_id = table.Column<int>(type: "int", nullable: false),
                    recipient_id = table.Column<int>(type: "int", nullable: false),
                    confirmation_id = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sent_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_delivered = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    delivered_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs_chat_console", x => x.id);
                    table.ForeignKey(
                        name: "FK_logs_chat_console_players_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_logs_chat_console_players_sender_id",
                        column: x => x.sender_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_logs_chat_console_recipient_id",
                table: "logs_chat_console",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_logs_chat_console_sender_id_recipient_id",
                table: "logs_chat_console",
                columns: new[] { "sender_id", "recipient_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs_chat_console");

            migrationBuilder.DropColumn(
                name: "perk_flags",
                table: "players");

            migrationBuilder.AlterColumn<bool>(
                name: "is_locked",
                table: "security_tickets",
                type: "tinyint(1)",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "wall_height",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "-1",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "users_now",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "users_max",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "25",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "trade_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "thickness_wall",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "thickness_floor",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "state",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "paint_wall",
                table: "rooms",
                type: "double",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "paint_landscape",
                table: "rooms",
                type: "double",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "paint_floor",
                table: "rooms",
                type: "double",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<int>(
                name: "mute_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "kick_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "hide_walls",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "chat_weight_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "chat_speed_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "chat_protection_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "2",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "chat_mode_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "chat_distance",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "50",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ban_type",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_walk_through",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_pets_eat",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_pets",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "allow_editing",
                table: "rooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "enabled",
                table: "room_models",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "door_y",
                table: "room_models",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "door_x",
                table: "room_models",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "door_rotation",
                table: "room_models",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "custom",
                table: "room_models",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.UpdateData(
                table: "room_chatlogs",
                keyColumn: "message",
                keyValue: null,
                column: "message",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "room_chatlogs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "players",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "gender",
                table: "players",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "figure",
                table: "players",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValueSql: "'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64'",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "amount",
                table: "player_currencies",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "navigator_flatcats",
                keyColumn: "global_category",
                keyValue: null,
                column: "global_category",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "global_category",
                table: "navigator_flatcats",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "navigator_flatcats",
                keyColumn: "automatic_category",
                keyValue: null,
                column: "automatic_category",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "automatic_category",
                table: "navigator_flatcats",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "z",
                table: "furniture_definitions",
                type: "double(10,3)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "double(10,3)");

            migrationBuilder.AlterColumn<int>(
                name: "y",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "x",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "usage_policy",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "furniture_definitions",
                type: "varchar(255)",
                nullable: false,
                defaultValueSql: "'s'",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "total_states",
                table: "furniture_definitions",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "logic",
                table: "furniture_definitions",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "'default'",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "can_walk",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_trade",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_stack",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_sit",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_sell",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_recycle",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_lay",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_group",
                table: "furniture_definitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<double>(
                name: "z",
                table: "furniture",
                type: "double(10,3)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "double(10,3)");

            migrationBuilder.AlterColumn<int>(
                name: "y",
                table: "furniture",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "x",
                table: "furniture",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "direction",
                table: "furniture",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "unique_size",
                table: "catalog_products",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "unique_remaining",
                table: "catalog_products",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "catalog_products",
                type: "int",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "visible",
                table: "catalog_pages",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.UpdateData(
                table: "catalog_pages",
                keyColumn: "text_data",
                keyValue: null,
                column: "text_data",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "text_data",
                table: "catalog_pages",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "layout",
                table: "catalog_pages",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "'default_3x3'",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "catalog_pages",
                keyColumn: "image_data",
                keyValue: null,
                column: "image_data",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "image_data",
                table: "catalog_pages",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "icon",
                table: "catalog_pages",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "visible",
                table: "catalog_offers",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "cost_currency",
                table: "catalog_offers",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "cost_credits",
                table: "catalog_offers",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "club_level",
                table: "catalog_offers",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "can_gift",
                table: "catalog_offers",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "can_bundle",
                table: "catalog_offers",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.CreateTable(
                name: "player_perks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    BUILDER_AT_WORK = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CALL_ON_HELPERS = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CAMERA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CITIZEN = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HABBO_CLUB_OFFER_BETA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JUDGE_CHAT_REVIEWS = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MOUSE_ZOOM = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NAVIGATOR_PHASE_TWO_2014 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NAVIGATOR_ROOM_THUMBNAIL_CAMERA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TRADE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UNITY_TRADE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    USE_GUIDE_TOOL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VOTE_IN_COMPETITIONS = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_perks", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_perks_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_player_perks_player_id",
                table: "player_perks",
                column: "player_id",
                unique: true);
        }
    }
}
