using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Database.Entities.Room
{
    [Table("room_models")]
    public class RoomModelEntity : Entity
    {
        [Column("name"), Required]
        public string Name { get; set; }

        [Column("model"), Required]
        public string Model { get; set; }

        [Column("door_x")]
        public int DoorX { get; set; }

        [Column("door_y")]
        public int DoorY { get; set; }

        [Column("door_rotation")]
        public Rotation DoorRotation { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }

        [Column("custom")]
        public bool Custom { get; set; }
    }
}
