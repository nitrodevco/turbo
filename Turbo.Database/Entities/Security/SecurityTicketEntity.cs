using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Security;

[Table("security_tickets")]
[Index(nameof(PlayerEntityId), IsUnique = true)]
[Index(nameof(Ticket), IsUnique = true)]
public class SecurityTicketEntity : Entity
{
    [Column("player_id")] public int PlayerEntityId { get; set; }

    [Column("ticket")] [Required] public string Ticket { get; set; }

    [Column("ip_address")] [Required] public string IpAddress { get; set; }

    [Column("is_locked")]
    [DefaultValueSql("0")]
    public bool? IsLocked { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }
}