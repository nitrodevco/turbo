using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record RentableSpaceRentFailedMessage : IComposer
    {
        public int Reason { get; init; }
    }
}
