using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Database.Attributes;
using Turbo.Core.Database.Entities.Navigator;
using Turbo.Core.Database.Entities.Players;
using Turbo.Core.Game.Rooms.Constants;

namespace Turbo.Core.Database.Entities.Room;

[Table("rooms")]
public class RoomEntity : Entity
{
    [Column("name")] [Required] public string Name { get; set; }

    [Column("description")] public string? Description { get; set; }

    [Column("player_id")] [Required] public int PlayerEntityId { get; set; }

    [Column("state")]
    [Required]
    [DefaultValueSql(RoomStateType.Open)] // RoomStateType.Open
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomStateType RoomState { get; set; }

    [Column("password")] public string? Password { get; set; }

    [Column("model_id")] [Required] public int RoomModelEntityId { get; set; }

    [Column("category_id")] public int? NavigatorCategoryEntityId { get; set; }

    [Column("users_now")]
    [Required]
    [DefaultValueSql(0)]
    public int UsersNow { get; set; }

    [Column("users_max")]
    [Required]
    [DefaultValueSql(25)]
    public int UsersMax { get; set; }

    [Column("paint_wall")]
    [Required]
    [DefaultValueSql(0.0d)]
    public double PaintWall { get; set; }

    [Column("paint_floor")]
    [Required]
    [DefaultValueSql(0.0d)]
    public double PaintFloor { get; set; }

    [Column("paint_landscape")]
    [Required]
    [DefaultValueSql(0.0d)]
    public double PaintLandscape { get; set; }

    [Column("wall_height")]
    [Required]
    [DefaultValueSql(-1)]
    public int WallHeight { get; set; }

    [Column("hide_walls")]
    [Required]
    [DefaultValueSql(false)]
    public bool? HideWalls { get; set; }

    [Column("thickness_wall")]
    [Required]
    [DefaultValueSql(RoomThicknessType.Normal)] // RoomThicknessType.Normal
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomThicknessType ThicknessWall { get; set; }

    [Column("thickness_floor")]
    [Required]
    [DefaultValueSql(RoomThicknessType.Normal)] // RoomThicknessType.Normal
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomThicknessType ThicknessFloor { get; set; }

    [Column("allow_walk_through")]
    [Required]
    [DefaultValueSql(true)]
    public bool? AllowWalkThrough { get; set; }

    [Column("allow_editing")]
    [Required]
    [DefaultValueSql(true)]
    public bool? AllowEditing { get; set; }

    [Column("allow_pets")]
    [Required]
    [DefaultValueSql(false)]
    public bool? AllowPets { get; set; }

    [Column("allow_pets_eat")]
    [Required]
    [DefaultValueSql(false)]
    public bool? AllowPetsEat { get; set; }

    [Column("trade_type")]
    [Required]
    [DefaultValueSql(RoomTradeType.Disabled)] // RoomTradeType.Disabled
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomTradeType TradeType { get; set; }

    [Column("mute_type")]
    [Required]
    [DefaultValueSql(RoomMuteType.None)] // RoomMuteType.None
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomMuteType MuteType { get; set; }

    [Column("kick_type")]
    [Required]
    [DefaultValueSql(RoomKickType.None)] // RoomKickType.None
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomKickType KickType { get; set; }

    [Column("ban_type")]
    [Required]
    [DefaultValueSql(RoomBanType.None)] // RoomBanType.None
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomBanType BanType { get; set; }

    [Column("chat_mode_type")]
    [Required]
    [DefaultValueSql(RoomChatModeType.FreeFlow)] // RoomChatModeType.FreeFlow
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomChatModeType ChatModeType { get; set; }

    [Column("chat_weight_type")]
    [Required]
    [DefaultValueSql(RoomChatWeightType.Normal)] // RoomChatWeightType.Normal
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomChatWeightType ChatWeightType { get; set; }

    [Column("chat_speed_type")]
    [Required]
    [DefaultValueSql(RoomChatSpeedType.Normal)] // RoomChatSpeedType.Normal
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public RoomChatSpeedType ChatSpeedType { get; set; }

    [Column("chat_protection_type")]
    [Required]
    [DefaultValueSql(RoomChatProtectionType.Minimal)] // RoomChatProtectionType.Minimal
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
    public NavigatorFlatCategoryEntity NavigatorFlatCategoryEntity { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomBanEntity> RoomBans { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomMuteEntity> RoomMutes { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomRightEntity> RoomRights { get; set; }

    [InverseProperty("RoomEntity")] public List<RoomChatlogEntity> RoomChats { get; set; }
}