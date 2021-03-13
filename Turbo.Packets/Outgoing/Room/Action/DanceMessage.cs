using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record DanceMessage : IComposer
    {
        public int UserId { get; init; }
        public int DanceStyle { get; init; }
    }
}
