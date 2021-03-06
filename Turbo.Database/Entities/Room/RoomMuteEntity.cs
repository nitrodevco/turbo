using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Room
{
    [Table("room_mutes")]
    public class RoomMuteEntity : Entity
    {
        [Column("room_id")]
        public int RoomEntityId { get; set; }

        public RoomEntity RoomEntity { get; set; }

        [Column("player_id")]
        public int PlayerEntityId { get; set; }

        public PlayerEntity PlayerEntity { get; set; }
    }
}
