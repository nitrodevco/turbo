using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Room
{
    [Table("chatlogs"), Index(nameof(RoomEntityId)), Index(nameof(PlayerEntityId)), Index(nameof(RecipientUserId))]
    public class ChatlogEntity : Entity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("room_id")]
        public int? RoomEntityId { get; set; }

        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [Column("recipient_user_id")]
        public int? RecipientUserId { get; set; }

        [Column("message"), Required, StringLength(100)]
        public string Message { get; set; }

        [Column("type"), Required]
        public string Type { get; set; }

        [Column("date_created"), Required]
        public DateTime DateCreated { get; set; }

        [Column("date_updated"), Required]
        public DateTime DateUpdated { get; set; }

        [ForeignKey(nameof(RoomEntityId))]
        public RoomEntity RoomEntity { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }

        [ForeignKey(nameof(RecipientUserId))]
        public PlayerEntity RecipientUser { get; set; }
    }
}