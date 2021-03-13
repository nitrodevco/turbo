using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record CustomStackingHeightUpdateMessage : IComposer
    {
        public int ItemId { get; init; }
        public int Height { get; init; }
    }
}
