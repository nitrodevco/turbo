using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Users;
public record GetRelationshipStatusInfoMessage : IMessageEvent
{
    public int PlayerId { get; init; }
}
