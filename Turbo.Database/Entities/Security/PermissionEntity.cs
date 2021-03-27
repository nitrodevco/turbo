using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Security
{
    [Table("permissions")]
    public class PermissionEntity : Entity
    {
        [Column("name"), Required]
        public string Name { get; set; }
    }
}
