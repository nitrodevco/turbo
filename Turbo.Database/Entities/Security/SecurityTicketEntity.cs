using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Security
{
    [Table("security_tickets")]
    public class SecurityTicketEntity : Entity
    {
        [Column("player_id")]
        public int PlayerEntityId { get; set; }

        public PlayerEntity PlayerEntity { get; set; }

        [Column("ticket")]
        public string Ticket { get; set; }

        [Column("ip_address")]
        public string IpAddress { get; set; }

        [Column("is_locked")]
        public bool IsLocked { get; set; }
    }
}
