using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class RoomTextSearchMessage : IMessageEvent
{
    public string Query { get; init; }
}