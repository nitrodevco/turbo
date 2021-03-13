using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record RequestSpamWallPostItMessage : IComposer
    {
        public int ItemId { get; init; }
        public string Location { get; init; }
    }
}
