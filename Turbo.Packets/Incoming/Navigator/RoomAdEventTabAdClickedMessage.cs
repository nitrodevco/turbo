using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class RoomAdEventTabAdClickedMessage : IMessageEvent
{
    public int FlatId { get; init; }
    public string RoomAdName { get; init; }
    public int RoomAdExpiresInMin { get; init; }
}