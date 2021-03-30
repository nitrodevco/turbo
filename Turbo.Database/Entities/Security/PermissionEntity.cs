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
        public string Name { get; set; }

        public List<RankPermissionEntity> RankPermissions { get; set; }

        public List<PlayerPermissionEntity> PlayerPermissions { get; set; }
    }
}
