using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Messenger
{
    [Table("messenger_friends"), Index(nameof(PlayerEntityId), nameof(FriendPlayerEntityId), IsUnique = true)]
    public class MessengerFriendEntity : Entity
    {
        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [Column("requested_id"), Required]
        public int FriendPlayerEntityId { get; set; }

        [Column("category_id")]
        public int? MessengerCategoryEntityId { get; set; }

        [Column("relation"), Required, DefaultValueSql("0")]
        public MessengerFriendRelationEnum RelationType { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }

        [ForeignKey(nameof(FriendPlayerEntityId))]
        public PlayerEntity FriendPlayerEntity { get; set; }

        [ForeignKey(nameof(MessengerCategoryEntityId))]
        public MessengerCategoryEntity MessengerCategoryEntity { get; set; }
    }
}
