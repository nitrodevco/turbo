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

        public List<SecurityTicketEntity> SecurityTickets { get; set; }

        public List<PermissionEntity> PlayerPermissions { get; set; }
    }
}
