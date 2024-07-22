using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Database.Attributes;
using Turbo.Database.Entities.Navigator;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Room;

[Table("rooms")]
public class RoomEntity : Entity
{
    [Column("name")] [Required] public string Name { get; set; }

    [Column("description")] public string? Description { get; set; }

    [Column("player_id")] [Required] public int PlayerEntityId { get; set; }

    [Column("state")]
    [Required]
    [DefaultValueSql("0")] // RoomStateType.Open
    public RoomStateType RoomState { get; set; }

    [Column("password")] public string? Password { get; set; }

    [Column("model_id")] [Required] public int RoomModelEntityId { get; set; }

    [Column("category_id")] public int? NavigatorCategoryEntityId { get; set; }

    [Column("users_now")]
    [Required]
    [DefaultValueSql("0")]
    public int UsersNow { get; set; }

    [Column("users_max")]
    [Required]
    [DefaultValueSql("25")]
    public int UsersMax { get; set; }

    [Column("paint_wall")]
    [Required]
    [DefaultValueSql("0")]
    public double PaintWall { get; set; }

    [Column("paint_floor")]
    [Required]
    [DefaultValueSql("0")]
    public double PaintFloor { get; set; }

    [Column("paint_landscape")]
    [Required]
    [DefaultValueSql("0")]
    public double PaintLandscape { get; set; }

    [Column("wall_height")]
    [Required]
    [DefaultValueSql("-1")]
    public int WallHeight { get; set; }

    [Column("hide_walls")]
    [Required]
    [DefaultValueSql("0")]
    public bool? HideWalls { get; set; }

    [Column("thickness_wall")]
    [Required]
    [DefaultValueSql("0")] // RoomThicknessType.Normal
    public RoomThicknessType ThicknessWall { get; set; }

    [Column("thickness_floor")]
    [Required]
    [DefaultValueSql("0")] // RoomThicknessType.Normal
    public RoomThicknessType ThicknessFloor { get; set; }

    [Column("allow_walk_through")]
    [Required]
    [DefaultValueSql("1")]
    public bool? AllowWalkThrough { get; set; }

    [Column("allow_editing")]
    [Required]
    [DefaultValueSql("1")]
    public bool? AllowEditing { get; set; }

    [Column("allow_pets")]
    [Required]
    [DefaultValueSql("0")]
    public bool? AllowPets { get; set; }

    [Column("allow_pets_eat")]
    [Required]
    [DefaultValueSql("0")]
    public bool? AllowPetsEat { get; set; }

    [Column("trade_type")]
    [Required]
    [DefaultValueSql("0")] // RoomTradeType.Disabled
    public RoomTradeType TradeType { get; set; }

    [Column("mute_type")]
    [Required]
    [DefaultValueSql("0")] // RoomMuteType.None
    public RoomMuteType MuteType { get; set; }

    [Column("kick_type")]
    [Required]
    [DefaultValueSql("0")] // RoomKickType.None
    public RoomKickType KickType { get; set; }

    [Column("ban_type")]
    [Required]
    [DefaultValueSql("0")] // RoomBanType.None
    public RoomBanType BanType { get; set; }

    [Column("chat_mode_type")]
    [Required]
    [DefaultValueSql("0")] // RoomChatModeType.FreeFlow
    public RoomChatModeType ChatModeType { get; set; }

    [Column("chat_weight_type")]
    [Required]
    [DefaultValueSql("1")] // RoomChatWeightType.Normal
    public RoomChatWeightType ChatWeightType { get; set; }

    [Column("chat_speed_type")]
    [Required]
    [DefaultValueSql("1")] // RoomChatSpeedType.Normal
    public RoomChatSpeedType ChatSpeedType { get; set; }

    [Column("chat_protection_type")]
    [Required]
    [DefaultValueSql("2")] // RoomChatProtectionType.Minimal
    public RoomChatProtectionType ChatProtectionType { get; set; }

    [Column("chat_distance")]
    [Required]
    [DefaultValueSql("50")]
    public int ChatDistance { get; set; }

    [Column("last_active")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime LastActive { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }

    [ForeignKey(nameof(RoomModelEntityId))]
    public RoomModelEntity RoomModelEntity { get; set; }

    [ForeignKey(nameof(NavigatorCategoryEntityId))]
    public NavigatorCategoryEntity NavigatorCategoryEntity { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomBanEntity> RoomBans { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomMuteEntity> RoomMutes { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomRightEntity> RoomRights { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomChatlogEntity> RoomChats { get; set; }
}