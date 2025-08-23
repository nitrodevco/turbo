using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Logs.Chat;

[Table("logs_chat_console")]
[Index(nameof(SenderEntityId), nameof(RecipientEntityId))]
public class ConsoleChatLogEntity : Entity
{
    [Column("sender_id")]
    [Required]
    public int SenderEntityId { get; set; }

    [Column("recipient_id")]
    [Required]
    public int RecipientEntityId { get; set; }

    //Add GroupId?

    [Column("confirmation_id")]
    public int ConfirmationId { get; set; }

    [Column("message")]
    [Required]
    public string Message { get; set; }

    [Column("sent_at")]
    public DateTime SentAt { get; set; }

    [Column("is_delivered")]
    [DefaultValueSql(false)]
    public bool IsDelivered { get; set; }

    [Column("delivered_at")]
    public DateTime? DeliveredAt { get; set; }

    [ForeignKey(nameof(SenderEntityId))]
    public PlayerEntity SenderEntity { get; set; }

    [ForeignKey(nameof(RecipientEntityId))]
    public PlayerEntity RecipientEntity { get; set; }
}
