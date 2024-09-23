using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class PlayerPerks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "player_perks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    CITIZEN = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VOTE_IN_COMPETITIONS = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TRADE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CALL_ON_HELPERS = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JUDGE_CHAT_REVIEWS = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NAVIGATOR_ROOM_THUMBNAIL_CAMERA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    USE_GUIDE_TOOL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MOUSE_ZOOM = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HABBO_CLUB_OFFER_BETA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NAVIGATOR_PHASE_TWO_2014 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UNITY_TRADE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BUILDER_AT_WORK = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CAMERA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_perks");
        }
    }
}
