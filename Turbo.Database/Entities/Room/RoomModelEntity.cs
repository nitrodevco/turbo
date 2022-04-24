using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Room
{
    [Table("room_models"), Index(nameof(Name), IsUnique = true)]
    public class RoomModelEntity : Entity
    {
        [Column("name"), Required]
        public string Name { get; set; }

        [Column("model"), Required]
        public string Model { get; set; }

        [Column("door_x"), DefaultValueSql("0")]
        public int DoorX { get; set; }

        [Column("door_y"), DefaultValueSql("0")]
        public int DoorY { get; set; }

        [Column("door_rotation"), DefaultValueSql("0")] // Rotation.North
        public Rotation DoorRotation { get; set; }

        [Column("enabled"), DefaultValueSql("1")]
        public bool? Enabled { get; set; }

        [Column("custom"), DefaultValueSql("0")]
        public bool? Custom { get; set; }
    }
}
