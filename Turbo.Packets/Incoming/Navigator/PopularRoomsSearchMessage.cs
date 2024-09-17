using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record PopularRoomsSearchMessage : IMessageEvent
{
    public string Query { get; init; }
    public int Unknown { get; init; }
}