using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Users;

public record GetExtendedProfileMessage : IMessageEvent
{
    public int PlayerId { get; init; }
}
