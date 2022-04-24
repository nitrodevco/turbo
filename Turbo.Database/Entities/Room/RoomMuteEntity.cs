using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Room
{
    [Table("room_mutes"), Index(nameof(RoomEntityId), nameof(PlayerEntityId), IsUnique = true)]
    public class RoomMuteEntity : Entity
    {
        [Column("room_id")]
        public int RoomEntityId { get; set; }

        [Column("player_id")]
        public int PlayerEntityId { get; set; }

        public RoomEntity RoomEntity { get; set; }

        public PlayerEntity PlayerEntity { get; set; }
    }
}
