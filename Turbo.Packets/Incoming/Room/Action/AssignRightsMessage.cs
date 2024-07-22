using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action;

public record AssignRightsMessage : IMessageEvent
{
    public int PlayerId { get; init; }
}