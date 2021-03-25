using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Navigator
{
    [Table("navigator_event_categories")]
    public class NavigatorEventCategoryEntity : Entity
    {
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("enabled")]
        public bool Enabled { get; set; }
    }
}
