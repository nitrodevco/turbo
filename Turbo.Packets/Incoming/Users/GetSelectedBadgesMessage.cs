using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Users;

public record GetSelectedBadgesMessage : IMessageEvent
{
    public int PlayerId { get; init; }
}