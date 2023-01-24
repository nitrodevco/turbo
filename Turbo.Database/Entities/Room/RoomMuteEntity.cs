using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Entities.Players;
using System.ComponentModel.DataAnnotations;

namespace Turbo.Database.Entities.Room
{
    [Table("room_mutes"), Index(nameof(RoomEntityId), nameof(PlayerEntityId), IsUnique = true)]
    public class RoomMuteEntity : Entity
    {
        [Column("room_id"), Required]
        public int RoomEntityId { get; set; }

        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [Column("date_expires"), Required]
        public DateTime DateExpires { get; set; }

        public RoomEntity RoomEntity { get; set; }

        public PlayerEntity PlayerEntity { get; set; }
    }
}
