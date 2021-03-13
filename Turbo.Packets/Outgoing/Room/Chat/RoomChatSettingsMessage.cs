using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record RoomChatSettings : IComposer
    {
        public int ChatMode { get; init; }
        public int ChatWeight { get; init; }
        public int ChatSpeed { get; init; }
        public int ChatDistance { get; init; }
        public int ChatProtection { get; init; }
    }
}
