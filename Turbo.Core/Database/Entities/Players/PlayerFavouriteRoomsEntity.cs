using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Core.Database.Entities.Players;

[Table("player_favourite_rooms")]
[Index(nameof(PlayerId), nameof(RoomId), IsUnique = true)]
[Index(nameof(RoomId))]
public sealed class PlayerFavouriteRoomsEntity
{
    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("room_id")]
    public int RoomId { get; set; }

    [Column("created_at", TypeName = "datetime(6)")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at", TypeName = "datetime(6)")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at", TypeName = "datetime(6)")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("PlayerId")]
    public PlayerEntity Player { get; set; }

    [ForeignKey("RoomId")]
    public RoomEntity Room { get; set; }
}