using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.ChatStyles
{
    [Table("player_owned_styles"), Index(nameof(PlayerEntityId), nameof(ChatStyleId), IsUnique = true)]
    public class PlayerOwnedStyleEntity : Entity
    {
        [Column("player_id"), Required] public int PlayerEntityId { get; set; }

        [Column("chatstyle_id"), Required] public int ChatStyleId { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }

        [ForeignKey(nameof(ChatStyleId))]
        public ChatStyleEntity ChatStyle { get; set; }
    }
}