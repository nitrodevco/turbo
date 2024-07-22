using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record RoomInfoUpdatedMessage : IComposer
{
    public int RoomId { get; init; }
}