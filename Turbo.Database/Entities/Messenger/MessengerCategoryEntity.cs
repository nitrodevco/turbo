using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities.Catalog;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Messenger
{
    [Table("messenger_categories"), Index(nameof(PlayerEntityId), nameof(Name), IsUnique = true)]
    public class MessengerCategoryEntity : Entity
    {
        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        [Column("name"), Required]
        public string Name { get; set; }

        [ForeignKey(nameof(PlayerEntityId))]
        public PlayerEntity PlayerEntity { get; set; }
    }
}
