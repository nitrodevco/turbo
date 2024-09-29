using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class CreatePlayerFavouriteRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "player_favourite_rooms",
                columns: table => new
                {
                    player_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_favourite_rooms", x => new { x.player_id, x.room_id });
                    table.ForeignKey(
                        name: "FK_player_favourite_rooms_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_favourite_rooms_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_player_favourite_rooms_player_id_room_id",
                table: "player_favourite_rooms",
                columns: new[] { "player_id", "room_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_favourite_rooms_room_id",
                table: "player_favourite_rooms",
                column: "room_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_favourite_rooms");
        }
    }
}
