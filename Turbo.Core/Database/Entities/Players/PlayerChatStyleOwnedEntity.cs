using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Core.Database.Entities.Players;

[Table("player_chat_styles_owned")]
[Index(nameof(PlayerEntityId), nameof(ChatStyleId), IsUnique = true)]
public class PlayerChatStyleOwnedEntity : Entity
{
    [Column("player_id")] [Required] public int PlayerEntityId { get; set; }

    [Column("chat_style_id")] [Required] public int ChatStyleId { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }

    [ForeignKey(nameof(ChatStyleId))] public PlayerChatStyleEntity ChatStyle { get; set; }
}