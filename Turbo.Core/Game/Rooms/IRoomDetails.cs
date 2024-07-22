using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Constants;

namespace Turbo.Core.Game.Rooms;

public interface IRoomDetails
{
    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public int PlayerId { get; }
    public string PlayerName { get; set; }
    public RoomStateType State { get; }
    public string Password { get; }
    public int ModelId { get; }
    public int UsersNow { get; set; }
    public int UsersMax { get; }
    public double PaintWall { get; }
    public double PaintFloor { get; }
    public double PaintLandscape { get; }
    public int WallHeight { get; }
    public bool HideWalls { get; }
    public RoomThicknessType ThicknessWall { get; }
    public RoomThicknessType ThicknessFloor { get; }
    public bool BlockingDisabled { get; }
    public bool AllowEditing { get; }
    public bool AllowPets { get; }
    public bool AllowPetsEat { get; }
    public RoomTradeType TradeType { get; }
    public RoomMuteType MuteType { get; }
    public RoomKickType KickType { get; }
    public RoomBanType BanType { get; }
    public RoomChatModeType ChatModeType { get; }
    public RoomChatWeightType ChatWeightType { get; }
    public RoomChatSpeedType ChatSpeedType { get; }
    public RoomChatProtectionType ChatProtectionType { get; }
    public int ChatDistance { get; }
    public DateTime LastActive { get; }
    public bool UpdateSettingsForPlayer(IPlayer player, IRoomSettings message);
}