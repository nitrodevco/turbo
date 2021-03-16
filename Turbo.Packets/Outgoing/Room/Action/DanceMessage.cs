using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action
{
    public record DanceMessage : IComposer
    {
        public int UserId { get; init; }
        public int DanceStyle { get; init; }
    }
}
