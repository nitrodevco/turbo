﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Security.Constants;

namespace Turbo.Database.Entities.Security
{
    [Table("rank_permissions")]
    public class RankPermissionEntity : Entity
    {
        [Column("rank_id"), Required]
        public int RankEntityId { get; set; }

        public RankEntity RankEntity { get; set; }

        [Column("permission_id"), Required]
        public int PermissionEntityId { get; set; }

        public PermissionEntity PermissionEntity { get; set; }

        [Column("required_rights"), Required]
        public PermissionRequiredRights RequiredRights { get; set; }
    }
}
