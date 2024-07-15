using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Entities.Players
{
    [Table("player_settings"), Index(nameof(PlayerEntityId), IsUnique = true)]
    public class PlayerSettingsEntity : Entity
    {
        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }
        
        [Column("chat_style")]
        public int ChatStyle { get; set; }
    }
}
