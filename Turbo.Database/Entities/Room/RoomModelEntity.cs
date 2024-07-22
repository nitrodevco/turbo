using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Room;

[Table("room_models")]
[Index(nameof(Name), IsUnique = true)]
public class RoomModelEntity : Entity
{
    [Column("name")] [Required] public string Name { get; set; }

    [Column("model")] [Required] public string Model { get; set; }

    [Column("door_x")]
    [Required]
    [DefaultValueSql("0")]
    public int DoorX { get; set; }

    [Column("door_y")]
    [Required]
    [DefaultValueSql("0")]
    public int DoorY { get; set; }

    [Column("door_rotation")]
    [Required]
    [DefaultValueSql("0")] // Rotation.North
    public Rotation DoorRotation { get; set; }

    [Column("enabled")]
    [Required]
    [DefaultValueSql("1")]
    public bool? Enabled { get; set; }

    [Column("custom")]
    [Required]
    [DefaultValueSql("0")]
    public bool? Custom { get; set; }
}