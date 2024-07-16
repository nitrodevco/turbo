using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Entities.Messenger
{
    [Table("messenger_requests"), Index(nameof(PlayerEntityId), nameof(RequestedPlayerEntityId), IsUnique = true)]
    public class MessengerRequestEntity : Entity
    {
        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [Column("requested_id"), Required]
        public int RequestedPlayerEntityId { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }

        [ForeignKey(nameof(RequestedPlayerEntityId))]
        public PlayerEntity RequestedPlayerEntity { get; set; }
    }
}
