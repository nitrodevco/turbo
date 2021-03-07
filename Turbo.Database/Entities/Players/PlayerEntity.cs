using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public List<SecurityTicketEntity> SecurityTickets { get; set; }
    }
}
