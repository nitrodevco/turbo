using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Security
{
    [Table("permissions")]
    public class PermissionEntity : Entity
    {
        [Column("name"), Required]
        [Key]
        public string Name { get; set; }

        public ICollection<RankEntity> Ranks { get; set; }
        public List<RankPermissionEntity> RankPermissions { get; set; }

        public ICollection<PlayerEntity> Players { get; set; }
        public List<PlayerPermissionEntity> PlayerPermissions { get; set; }
    }
}
