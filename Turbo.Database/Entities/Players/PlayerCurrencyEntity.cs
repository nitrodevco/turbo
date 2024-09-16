using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Entities.Players
{
    [Table("player_currencies"), Index(nameof(PlayerEntityId), nameof(Type), IsUnique = true)]
    public class PlayerCurrencyEntity : Entity
    {
        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [Column("type"), Required]
        public int Type { get; set; }

        [Column("amount"), Required, DefaultValueSql("0")]
        public int Amount { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }
    }
}
