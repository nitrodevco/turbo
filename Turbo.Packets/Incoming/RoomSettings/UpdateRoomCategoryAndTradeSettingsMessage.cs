using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.RoomSettings;

public record UpdateRoomCategoryAndTradeSettingsMessage : IMessageEvent
{
    public int RoomId { get; init; }
    public int CategoryId { get; init; }
    public RoomTradeType TradeType { get; init; }
}