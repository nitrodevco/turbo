using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Entities.Room
{
    [Table("rooms")]
    public class RoomEntity : Entity
    {
        [Column("name"), Required]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("player_id")]
        public int PlayerEntityId { get; set; }

        public PlayerEntity PlayerEntity { get; set; }

        [Column("state")]
        public RoomStateType RoomState { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("model_id")]
        public int RoomModelEntityId { get; set; }

        public RoomModelEntity RoomModelEntity { get; set; }

        [Column("users_now")]
        public int UsersNow { get; set; }

        [Column("users_max")]
        public int UsersMax { get; set; }

        [Column("paint_wall")]
        public double PaintWall { get; set; }

        [Column("paint_floor")]
        public double PaintFloor { get; set; }

        [Column("paint_landscape")]
        public double PaintLandscape { get; set; }

        [Column("wall_height")]
        public int WallHeight { get; set; }

        [Column("hide_walls")]
        public bool HideWalls { get; set; }

        [Column("thickness_wall")]
        public RoomThicknessType ThicknessWall { get; set; }

        [Column("thickness_floor")]
        public RoomThicknessType ThicknessFloor { get; set; }

        [Column("allow_walk_through")]
        public bool AllowWalkThrough { get; set; }

        [Column("allow_editing")]
        public bool AllowEditing { get; set; }

        [Column("allow_pets")]
        public bool AllowPets { get; set; }

        [Column("allow_pets_eat")]
        public bool AllowPetsEat { get; set; }

        [Column("trade_type")]
        public RoomTradeType TradeType { get; set; }

        [Column("mute_type")]
        public RoomMuteType MuteType { get; set; }

        [Column("kick_type")]
        public RoomKickType KickType { get; set; }

        [Column("ban_type")]
        public RoomBanType BanType { get; set; }

        [Column("chat_mode_type")]
        public RoomChatModeType ChatModeType { get; set; }

        [Column("chat_weight_type")]
        public RoomChatWeightType ChatWeightType { get; set; }

        [Column("chat_speed_type")]
        public RoomChatSpeedType ChatSpeedType { get; set; }

        [Column("chat_protection_type")]
        public RoomChatProtectionType ChatProtectionType { get; set; }

        [Column("chat_distance")]
        public int ChatDistance { get; set; }

        [Column("last_active")]
        public DateTime LastActive { get; set; }

        public List<RoomBanEntity> RoomBans { get; set; }

        public List<RoomMuteEntity> RoomMutes { get; set; }

        public List<RoomRightEntity> RoomRights { get; set; }
    }
}
