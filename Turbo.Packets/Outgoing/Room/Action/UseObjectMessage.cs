using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record UseObjectMessage : IComposer
    {
        public int UserId { get; init; }
        public int ItemType { get; init; }
    }
}
