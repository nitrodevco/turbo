using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Database.Attributes;
using Turbo.Core.Database.Entities.Players;
using Turbo.Core.Database.Entities.Room;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Database.Entities.Furniture;

[Table("furniture")]
public class FurnitureEntity : Entity
{
    [Column("player_id")] public int PlayerEntityId { get; set; }

    [Column("definition_id")] public int FurnitureDefinitionEntityId { get; set; }

    [Column("room_id")] public int? RoomEntityId { get; set; }

    [Column("x")][DefaultValueSql("0")] public int X { get; set; } = 0;

    [Column("y")][DefaultValueSql("0")] public int Y { get; set; } = 0;

    [Column("z", TypeName = "double(10,3)")]
    [DefaultValueSql(0.0d)]
    public double Z { get; set; }

    [Column("direction")]
    [DefaultValueSql(Rotation.North)] //Rotation.North
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Rotation Rotation { get; set; }

    [Column("wall_position")] public string? WallPosition { get; set; }

    [Column("stuff_data")] public string? StuffData { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }

    [ForeignKey(nameof(FurnitureDefinitionEntityId))]
    public FurnitureDefinitionEntity FurnitureDefinitionEntity { get; set; }

    [ForeignKey(nameof(RoomEntityId))] public RoomEntity RoomEntity { get; set; }
}