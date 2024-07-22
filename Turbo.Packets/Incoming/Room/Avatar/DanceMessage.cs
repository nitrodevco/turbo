using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Avatar;

public record DanceMessage : IMessageEvent
{
    public int Style { get; init; }
}