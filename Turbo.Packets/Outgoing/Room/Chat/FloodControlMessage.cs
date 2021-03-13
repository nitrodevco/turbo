using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record FloodControlMessage : IComposer
    {
        public int Seconds { get; init; }
    }
}
