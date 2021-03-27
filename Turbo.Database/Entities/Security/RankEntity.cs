using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Security
{
    [Table("ranks")]
    public class RankEntity : Entity
    {
        [Column("client_level"), Required]
        public int ClientLevel { get; set; }

        [Column("name"), Required]
        public string Name { get; set; }

        public List<PermissionEntity> RankPermissions { get; set; }
    }
}
