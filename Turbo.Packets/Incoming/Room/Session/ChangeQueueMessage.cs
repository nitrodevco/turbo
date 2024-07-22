using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Session;

public record ChangeQueueMessage : IMessageEvent
{
    public int TargetQueue { get; init; }
}