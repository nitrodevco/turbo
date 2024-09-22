using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record YouAreNotSpectatorMessage : IComposer
{
    public int flatId { get; init; }
}