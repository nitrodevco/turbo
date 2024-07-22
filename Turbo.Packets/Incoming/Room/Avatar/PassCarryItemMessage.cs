using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Avatar;

public record PassCarryItemMessage : IMessageEvent
{
    public int UserId { get; init; }
}