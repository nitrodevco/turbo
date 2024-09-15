using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Attributes;
using Turbo.Core.Database.Entities.Furniture;
using Turbo.Core.Database.Entities.Messenger;
using Turbo.Core.Database.Entities.Room;
using Turbo.Core.Database.Entities.Security;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Database.Entities.Players;

[Table("players")]
[Index(nameof(Name), IsUnique = true)]
public class PlayerEntity : Entity
{
    [Column("name")] [Required] public string Name { get; set; }

    [Column("motto")] public string? Motto { get; set; }

    [Column("figure")]
    [Required]
    [MaxLength(100)]
    [DefaultValueSql("'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64'")]
    public string Figure { get; set; }

    [Column("gender")]
    [Required]
    [DefaultValueSql("0")] // AvatarGender.Male
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public AvatarGender Gender { get; set; }

    [Column("status")]
    [Required]
    [DefaultValueSql(PlayerStatusEnum.Offline)] // PlayerStatus.Offline
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public PlayerStatusEnum PlayerStatus { get; set; }

    [Column("room_chat_style_id")] public int? RoomChatStyleId { get; set; }

    [InverseProperty("PlayerEntity")] public List<PlayerBadgeEntity> PlayerBadges { get; set; }

    [InverseProperty("PlayerEntity")] public List<PlayerCurrencyEntity> PlayerCurrencies { get; set; }

    [InverseProperty("PlayerEntity")] public List<PlayerChatStyleOwnedEntity> PlayerOwnedChatStyles { get; set; }

    [InverseProperty("PlayerEntity")] public List<FurnitureEntity> Furniture { get; set; }

    [InverseProperty("PlayerEntity")] public List<MessengerCategoryEntity> MessengerCategories { get; set; }

    [InverseProperty("PlayerEntity")] public List<MessengerFriendEntity> MessengerFriends { get; set; }

    [InverseProperty("RequestedPlayerEntity")]
    public List<MessengerRequestEntity> MessengerRequests { get; set; }

    [InverseProperty("PlayerEntity")] public List<MessengerRequestEntity> MessengerRequestsSent { get; set; }

    [InverseProperty("PlayerEntity")] public List<SecurityTicketEntity> SecurityTickets { get; set; }

    [InverseProperty("PlayerEntity")] public List<RoomEntity> Rooms { get; set; }

    [InverseProperty("PlayerEntity")] public List<RoomBanEntity> RoomBans { get; set; }

    [InverseProperty("PlayerEntity")] public List<RoomMuteEntity> RoomMutes { get; set; }

    [InverseProperty("PlayerEntity")] public List<RoomRightEntity> RoomRights { get; set; }

    [InverseProperty("PlayerEntity")] public List<RoomChatlogEntity> RoomChatlogs { get; set; }
}