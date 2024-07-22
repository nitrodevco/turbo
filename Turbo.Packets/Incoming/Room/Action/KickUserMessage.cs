using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action;

public record KickUserMessage : IMessageEvent
{
    public int PlayerId { get; init; }
}