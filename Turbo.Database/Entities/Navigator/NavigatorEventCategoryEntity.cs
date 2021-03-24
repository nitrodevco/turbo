using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Navigator
{
    [Table("navigator_event_categories")]
    public class NavigatorEventCategoryEntity : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool Enabled { get; set; }
    }
}
