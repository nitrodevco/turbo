using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities.Security;

namespace Turbo.Database.Entities.Players
{
    [Table("players")]
    public class PlayerEntity : Entity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("motto")]
        public string Motto { get; set; }

        [Column("figure")]
        public string Figure { get; set; }

        [Column("gender")]
        public string Gender { get; set; }

        public List<SecurityTicketEntity> SecurityTickets { get; set; }
    }
}
