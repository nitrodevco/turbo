using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Entities.Players;

[Table("player_chat_styles")]
[Index(nameof(ClientStyleId), IsUnique = true)]
public class PlayerChatStyleEntity : Entity
{
    [Column("client_style_id")] [Required] public int ClientStyleId { get; set; }

    // TODO a column with a optional required permission id to access this chat style

    public List<PlayerChatStyleOwnedEntity> OwnedChatStyles { get; set; }
}