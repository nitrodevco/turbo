using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record RentableSpaceRentOkMessage : IComposer
    {
        public int ExpiryTime { get; init; }
    }
}
