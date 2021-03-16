using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action
{
    public record SleepMessage : IComposer
    {
        public int UserId { get; init; }
        public bool Sleeping { get; init; }
    }
}
