using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record YoutubeStateChangeMessage : IComposer
    {
        public int ItemId { get; init; }
        public int State { get; init; }
    }
}
