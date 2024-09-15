using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Core.Database.Entities.Room;

[Table("room_chatlogs")]
public class RoomChatlogEntity : Entity
{
    [Column("room_id")] [Required] public int RoomEntityId { get; set; }

    [Column("player_id")] [Required] public int PlayerEntityId { get; set; }

    [Column("target_player_id")] public int? TargetPlayerEntityId { get; set; }

    [Column("message")]
    [StringLength(100)]
    public string Message { get; set; }

    [ForeignKey(nameof(RoomEntityId))] public RoomEntity RoomEntity { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }

    [ForeignKey(nameof(TargetPlayerEntityId))]
    public PlayerEntity TargetPlayerEntity { get; set; }
}