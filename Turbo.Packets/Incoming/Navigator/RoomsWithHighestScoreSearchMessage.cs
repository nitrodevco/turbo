using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record RoomsWithHighestScoreSearchMessage : IMessageEvent
{
    public int Unknown { get; init; }
}