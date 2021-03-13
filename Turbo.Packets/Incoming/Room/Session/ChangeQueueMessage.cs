using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Pets
{
    public record ChangeQueueMessage : IMessageEvent
    {
        public int TargetQueue { get; init; }
    }
}
