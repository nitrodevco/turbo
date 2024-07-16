using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Turbo.Database.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Entities.Players
{
    [Table("player_settings"), Index(nameof(PlayerEntityId), IsUnique = true)]
    public class PlayerSettingsEntity : Entity
    {
        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }
    }
}
