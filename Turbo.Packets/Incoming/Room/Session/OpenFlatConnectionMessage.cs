using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Session;

public record OpenFlatConnectionMessage : IMessageEvent
{
    public int RoomId { get; init; }
    public string Password { get; init; }
}