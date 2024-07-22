using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Chat;

public record FloodControlMessage : IComposer
{
    public int Seconds { get; init; }
}