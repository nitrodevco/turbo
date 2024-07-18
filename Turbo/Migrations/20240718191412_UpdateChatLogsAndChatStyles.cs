using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatLogsAndChatStyles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatlogs");

            migrationBuilder.DropTable(
                name: "player_owned_styles");

            migrationBuilder.DropTable(
                name: "player_settings");

            migrationBuilder.DropTable(
                name: "chat_styles");

            migrationBuilder.AddColumn<int>(
                name: "room_chat_style_id",
                table: "players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "player_chat_styles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_style_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_chat_styles", x => x.id);
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
                    message = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                name: "player_chat_styles_owned",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    chat_style_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_chat_styles_owned");

            migrationBuilder.DropTable(
                name: "room_chatlogs");

            migrationBuilder.DropTable(
                name: "player_chat_styles");

            migrationBuilder.DropColumn(
                name: "room_chat_style_id",
                table: "players");

            migrationBuilder.CreateTable(
                name: "chat_styles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankRequirement = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_styles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chatlogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    recipient_user_id = table.Column<int>(type: "int", nullable: true),
                    room_id = table.Column<int>(type: "int", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    message = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatlogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_chatlogs_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chatlogs_players_recipient_user_id",
                        column: x => x.recipient_user_id,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_chatlogs_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    chat_style = table.Column<int>(type: "int", nullable: false),
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
                name: "player_owned_styles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    chatstyle_id = table.Column<int>(type: "int", nullable: false),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_owned_styles", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_owned_styles_chat_styles_chatstyle_id",
                        column: x => x.chatstyle_id,
                        principalTable: "chat_styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_owned_styles_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_chatlogs_player_id",
                table: "chatlogs",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_chatlogs_recipient_user_id",
                table: "chatlogs",
                column: "recipient_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_chatlogs_room_id",
                table: "chatlogs",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_owned_styles_chatstyle_id",
                table: "player_owned_styles",
                column: "chatstyle_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_owned_styles_player_id_chatstyle_id",
                table: "player_owned_styles",
                columns: new[] { "player_id", "chatstyle_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_settings_player_id",
                table: "player_settings",
                column: "player_id",
                unique: true);
        }
    }
}
