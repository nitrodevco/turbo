using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Core.Database.Entities.Players;

[Table("player_badges")]
[Index(nameof(PlayerEntityId), nameof(BadgeCode), IsUnique = true)]
public class PlayerBadgeEntity : Entity
{
    [Column("player_id")] [Required] public int PlayerEntityId { get; set; }

    [Column("badge_code")] [Required] public string BadgeCode { get; set; }

    [Column("slot_id")] public int? SlotId { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }
}