using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record CanCreateRoomMessage : IComposer
{
    public bool MaxRoomsReached { get; init; }
    public int MaxRooms { get; init; }
}
