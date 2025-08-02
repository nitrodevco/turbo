using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Attributes;

namespace Turbo.Core.Database.Entities.Players;

[Table("player_currencies")]
[Index(nameof(PlayerEntityId), nameof(Type), IsUnique = true)]
public class PlayerCurrencyEntity : Entity
{
    [Column("player_id")][Required] public int PlayerEntityId { get; set; }

    [Column("type")][Required] public string Type { get; set; }

    [Column("amount")]
    [Required]
    [DefaultValueSql(0)]
    public int Amount { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }
}