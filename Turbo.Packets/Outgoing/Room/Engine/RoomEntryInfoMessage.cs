using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record RoomEntryInfoMessage : IComposer
{
    public int RoomId { get; init; }
    public bool Owner { get; init; }
}