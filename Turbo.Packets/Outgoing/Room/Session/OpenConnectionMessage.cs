using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record OpenConnectionMessage : IComposer
{
    public int RoomId { get; set; }
}