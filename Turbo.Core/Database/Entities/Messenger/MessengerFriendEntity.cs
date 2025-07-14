using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Attributes;
using Turbo.Core.Database.Entities.Players;
using Turbo.Core.Game.Messenger.Constants;

namespace Turbo.Core.Database.Entities.Messenger;

[Table("messenger_friends")]
[Index(nameof(PlayerEntityId), nameof(FriendPlayerEntityId), IsUnique = true)]
public class MessengerFriendEntity : Entity
{
    [Column("player_id")][Required] public int PlayerEntityId { get; set; }

    [Column("requested_id")][Required] public int FriendPlayerEntityId { get; set; }

    [Column("category_id")] public int? MessengerCategoryEntityId { get; set; }

    [Column("relation")]
    [Required]
    [DefaultValueSql(MessengerFriendRelationEnum.Zero)]
    public MessengerFriendRelationEnum RelationType { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }

    [ForeignKey(nameof(FriendPlayerEntityId))]
    public PlayerEntity FriendPlayerEntity { get; set; }

    [ForeignKey(nameof(MessengerCategoryEntityId))]
    public MessengerCategoryEntity MessengerCategoryEntity { get; set; }
}