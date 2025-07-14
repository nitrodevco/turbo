using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Attributes;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Database.Entities.Room;

[Table("room_models")]
[Index(nameof(Name), IsUnique = true)]
public class RoomModelEntity : Entity
{
    [Column("name")][Required] public string Name { get; set; }

    [Column("model")][Required] public string Model { get; set; }

    [Column("door_x")]
    [Required]
    [DefaultValueSql(0)]
    public int DoorX { get; set; }

    [Column("door_y")]
    [Required]
    [DefaultValueSql(0)]
    public int DoorY { get; set; }

    [Column("door_rotation")]
    [Required]
    [DefaultValueSql(Rotation.North)] // Rotation.North
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Rotation DoorRotation { get; set; }

    [Column("enabled")]
    [Required]
    [DefaultValueSql(true)]
    public bool? Enabled { get; set; }

    [Column("custom")]
    [Required]
    [DefaultValueSql(false)]
    public bool? Custom { get; set; }
}