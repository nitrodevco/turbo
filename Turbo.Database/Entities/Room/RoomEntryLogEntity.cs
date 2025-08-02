using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Core.Database.Entities.Room;

[Table("room_entry_logs")]
public class RoomEntryLogEntity : Entity
{
    [Column("room_id")]
    [Required]
    public int RoomEntityId { get; set; }

    [Column("player_id")]
    [Required]
    public int PlayerEntityId { get; set; }

    [ForeignKey(nameof(RoomEntityId))]
    public RoomEntity RoomEntity { get; set; }

    [ForeignKey(nameof(PlayerEntityId))]
    public PlayerEntity PlayerEntity { get; set; }
}
