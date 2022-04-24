using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Turbo.Database.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Entities.Players
{
    [Table("player_settings"), Index(nameof(PlayerEntityId), IsUnique = true)]
    public class PlayerSettingsEntity : Entity
    {
        [Column("player_id")]
        public int PlayerEntityId { get; set; }

        public PlayerEntity PlayerEntity { get; set; }
    }
}
