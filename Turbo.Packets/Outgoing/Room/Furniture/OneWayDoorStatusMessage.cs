using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record OneWayDoorStatus : IComposer
    {
        public int Id { get; init; }
        public int Status { get; init; }
    }
}
