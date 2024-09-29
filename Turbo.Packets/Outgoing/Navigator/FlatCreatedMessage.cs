using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record FlatCreatedMessage : IComposer
{
    public int RoomId { get; init; }
    public string RoomName { get; init; }
}
