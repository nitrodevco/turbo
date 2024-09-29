using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record FlatAccessibleMessage : IComposer
{
    public int RoomId { get; init; }
    public string Username { get; init; }
}