using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record DiceValueMessage : IComposer
    {
        public int ItemId { get; init; }
        public int Value { get; init; }
    }
}
