using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Room
{
    [Table("room_models")]
    public class RoomModelEntity : Entity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("door_x")]
        public int DoorX { get; set; }

        [Column("door_y")]
        public int DoorY { get; set; }

        [Column("door_direction")]
        public int DoorDirection { get; set; }

        [Column("model")]
        public string Model { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }

        [Column("custom")]
        public bool Custom { get; set; }
    }
}
