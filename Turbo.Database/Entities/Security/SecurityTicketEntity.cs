using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Security
{
    [Table("security_tickets")]
    public class SecurityTicketEntity : Entity
    {
        public PlayerEntity PlayerEntity { get; set; }

        [Column("ticket")]
        public string Ticket { get; set; }

        [Column("ip_address")]
        public string IpAddress { get; set; }

        [Column("is_locked")]
        public bool IsLocked { get; set; }
    }
}
