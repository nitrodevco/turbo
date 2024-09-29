using Turbo.Core.Game.Navigator.Constants;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record CreateFlatMessage : IMessageEvent
{
    public string FlatName { get; init; }
    public string FlatDescription { get; init; }
    public string FlatModelName { get; init; }
    public int CategoryID { get; init; }
    public int MaxPlayers { get; init; }
    public RoomTradeType TradeSetting { get; init; }
}