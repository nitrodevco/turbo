using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Navigator;

[Table("navigator_event_categories")]
public class NavigatorEventCategoryEntity : Entity
{
    [Column("name")] [Required] public string Name { get; set; }

    [Column("enabled")] [Required] public bool Enabled { get; set; }
}