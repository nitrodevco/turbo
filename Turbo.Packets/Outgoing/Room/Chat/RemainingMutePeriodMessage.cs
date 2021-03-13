using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record RemaningMutePeriodMessage : IComposer
    {
        public int MuteSecondsRemaining { get; init; }
    }
}
