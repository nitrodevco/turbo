using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Room
{
    [Table("room_bans")]
    public class RoomBanEntity : Entity
    {
        [Column("room_id")]
        public int RoomEntityId { get; set; }
         
        public RoomEntity RoomEntity { get; set; }

        [Column("player_id")]
        public int PlayerEntityId { get; set; }

        public PlayerEntity PlayerEntity { get; set; }
    }
}
