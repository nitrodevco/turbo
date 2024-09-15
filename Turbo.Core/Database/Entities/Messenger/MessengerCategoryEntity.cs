using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Core.Database.Entities.Messenger;

[Table("messenger_categories")]
[Index(nameof(PlayerEntityId), nameof(Name), IsUnique = true)]
public class MessengerCategoryEntity : Entity
{
    [Column("player_id")] [Required] public int PlayerEntityId { get; set; }

    [Column("name")] [Required] public string Name { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }
}