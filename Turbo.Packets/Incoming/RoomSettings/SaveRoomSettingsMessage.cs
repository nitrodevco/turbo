using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.RoomSettings;

public record SaveRoomSettingsMessage : IRoomSettings, IMessageEvent
{
    public int RoomId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public RoomStateType State { get; init; }
    public string Password { get; init; }
    public int UsersMax { get; init; }
    public int CategoryId { get; init; }
    public IList<string> Tags { get; init; }
    public RoomTradeType TradeType { get; init; }
    public bool AllowPets { get; init; }
    public bool AllowPetsEat { get; init; }
    public bool BlockingDisabled { get; init; }
    public bool HideWalls { get; init; }
    public RoomThicknessType WallThickness { get; init; }
    public RoomThicknessType FloorThickness { get; init; }
    public RoomMuteType MuteType { get; init; }
    public RoomKickType KickType { get; init; }
    public RoomBanType BanType { get; init; }
    public RoomChatModeType ChatModeType { get; init; }
    public RoomChatWeightType ChatWeightType { get; init; }
    public RoomChatSpeedType ChatSpeed { get; init; }
    public int ChatDistance { get; init; }
    public RoomChatProtectionType ChatProtectionType { get; init; }
    public bool AllowNavigatorDynamicCategories { get; init; }
}