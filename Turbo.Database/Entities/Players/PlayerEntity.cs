using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Entities.Security;

namespace Turbo.Database.Entities.Players
{
    [Table("players")]
    public class PlayerEntity : Entity
    {
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("motto")]
        public string Motto { get; set; }

        [Required]
        [Column("figure")]
        public string Figure { get; set; }

        [Required]
        [Column("gender")]
        public string Gender { get; set; }

        [Column("rank")]
        public int RankEntityId { get; set; }
        public RankEntity Rank { get; set; }

        public List<SecurityTicketEntity> SecurityTickets { get; set; }

        public List<PlayerPermissionEntity> PlayerPermissions { get; set; }
        public ICollection<PermissionEntity> Permissions { get; set; }
    }
}
