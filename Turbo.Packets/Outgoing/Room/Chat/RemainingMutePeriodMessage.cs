using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Chat;

public record RemaningMutePeriodMessage : IComposer
{
    public int MuteSecondsRemaining { get; init; }
}