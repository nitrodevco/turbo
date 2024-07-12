using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class AddChatlogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chatlogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: true),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    recipient_user_id = table.Column<int>(type: "int", nullable: true),
                    message = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatlogs");
        }
    }
}
