using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Security.Permissions;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Security
{
    [Table("ranks")]
    public class RankEntity : Entity
    {
        [Column("client_level"), Required]
        public int ClientLevel { get; set; }

        [Column("name"), Required]
        public string Name { get; set; }

        public ICollection<PermissionEntity> Permissions { get; set; }
        public List<RankPermissionEntity> RankPermissions { get; set; }
        public List<PlayerEntity> Players;
    }
}
