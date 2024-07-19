using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo.Main.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialChatStyles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "chat_styles",
                columns: new[] { "Id", "Name", "Description" },
                values: new object[,]
                {
                    { 1, "Notice", "Exclamation mark chat bubble used for respect or wired notifications." },
                    { 2, "Bot", "Chat bubble used by automated bots." },
                    { 3, "Red", "Red colored standard chat bubble." },
                    { 4, "Blue", "Blue colored standard chat bubble." },
                    { 5, "Yellow", "Yellow colored standard chat bubble." },
                    { 6, "Green", "Green colored standard chat bubble." },
                    { 7, "Gray", "Gray colored standard chat bubble." },
                    { 8, "Mystic Habbo", "Mystic themed chat bubble for users." },
                    { 9, "Zombie", "Zombie arm chat bubble introduced for HabboWeen." },
                    { 10, "Skeleton", "Skeleton themed chat bubble launched for HabboWeen." },
                    { 11, "Light Blue", "Light blue colored standard chat bubble." },
                    { 12, "Pink", "Pink colored standard chat bubble." },
                    { 13, "Purple", "Purple colored standard chat bubble." },
                    { 14, "Orange", "Orange colored standard chat bubble." },
                    { 15, "Turquoise", "Turquoise colored standard chat bubble." },
                    { 16, "Love Heart", "Love heart chat bubble introduced for Valentine's Day." },
                    { 17, "Roses", "Rose-themed chat bubble introduced for Valentine's Day." },
                    { 18, "Plaster", "Plaster or 'Band-Aid' styled chat bubble." },
                    { 19, "Piglet", "Piglet themed chat bubble." },
                    { 20, "Puppy", "Puppy themed chat bubble." },
                    { 21, "Lizard Beam", "Lizard beam themed chat bubble." },
                    { 22, "Fire Breathing Dragon", "Dragon breathing fire themed chat bubble." },
                    { 23, "Staff", "Staff badge themed chat bubble used by hotel staff." },
                    { 24, "Bats", "Bats themed chat bubble introduced during HabboWeen." },
                    { 25, "Oldschool Console", "Old school Habbo console themed chat bubble." },
                    { 26, "Steampunk", "Steampunk inspired chat bubble." },
                    { 27, "Lightning Shower", "Thunder and lightning themed chat bubble." },
                    { 28, "Parrot", "Pirate's parrot themed chat bubble." },
                    { 29, "Pirates", "Pirate themed chat bubble with swords." },
                    { 30, "Bot - Light", "Light bot themed chat bubble used by bots." },
                    { 31, "Bot - Dark", "Dark bot themed chat bubble used by bots." },
                    { 32, "Skeleton Pillory", "Skeleton in a pillory themed chat bubble." },
                    { 33, "Frank's Hat Notice", "Frank's Hat themed notice bubble for system announcements." },
                    { 34, "New Notice", "Exclamation mark notice bubble for system announcements." },
                    { 35, "Goat", "Goat themed chat bubble." },
                    { 36, "Santa Bot", "Chat bubble used for Santa Bot during Christmas." },
                    { 37, "Ambassador", "Ambassador badge themed chat bubble used by Ambassadors." },
                    { 38, "Speaker", "Speaker themed chat bubble." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
