using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Security.Constants;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Security
{
    [Table("player_permissions")]
    public class PlayerPermissionEntity : Entity
    {
        [Column("player_id"), Required]
        public int PlayerEntityId { get; set; }

        public PlayerEntity PlayerEntity { get; set; }

        [Column("permission_id"), Required]
        public int PermissionEntityId { get; set; }

        public PermissionEntity PermissionEntity { get; set; }

        [Column("required_rights", TypeName = "nvarchar(24)")]
        public PermissionRequiredRights RequiredRights { get; set; }
    }
}
