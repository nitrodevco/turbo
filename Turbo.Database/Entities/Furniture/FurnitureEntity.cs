using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Database.Entities.Players;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Entities.Furniture
{
    [Table("furniture")]
    public class FurnitureEntity : Entity
    {
        [Column("player_id")]
        public int PlayerEntityId { get; set; }

        public PlayerEntity PlayerEntity { get; set; }

        [Column("definition_id")]
        public int FurnitureDefinitionEntityId { get; set; }

        public FurnitureDefinitionEntity FurnitureDefinitionEntity { get; set; }

        [Column("room_id")]
        public int? RoomEntityId { get; set; }

        public RoomEntity RoomEntity { get; set; }

        [Column("x")]
        public int X { get; set; }

        [Column("y")]
        public int Y { get; set; }

        [Column("z")]
        public double Z { get; set; }

        [Column("direction")]
        public Rotation Rotation { get; set; }

        [Column("wall_position")]
        public string? WallPosition { get; set; }

        [Column("stuff_data")]
        public string? StuffData { get; set; }
    }
}
