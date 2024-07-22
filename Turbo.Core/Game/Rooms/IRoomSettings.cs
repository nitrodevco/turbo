using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Constants;

namespace Turbo.Core.Game.Rooms;

public interface IRoomSettings
{
    public int RoomId { get; }
    public string Name { get; }
    public string Description { get; }
    public RoomStateType State { get; }
    public string Password { get; }
    public int UsersMax { get; }
    public int CategoryId { get; }
    public IList<string> Tags { get; }
    public RoomTradeType TradeType { get; }
    public bool AllowPets { get; }
    public bool AllowPetsEat { get; }
    public bool BlockingDisabled { get; }
    public bool HideWalls { get; }
    public RoomThicknessType WallThickness { get; }
    public RoomThicknessType FloorThickness { get; }
    public RoomMuteType MuteType { get; }
    public RoomKickType KickType { get; }
    public RoomBanType BanType { get; }
    public RoomChatModeType ChatModeType { get; }
    public RoomChatWeightType ChatWeightType { get; }
    public RoomChatSpeedType ChatSpeed { get; }
    public int ChatDistance { get; }
    public RoomChatProtectionType ChatProtectionType { get; }
    public bool AllowNavigatorDynamicCategories { get; }
}