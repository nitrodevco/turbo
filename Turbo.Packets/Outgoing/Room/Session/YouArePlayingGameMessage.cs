using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record YouArePlayingGameMessage : IComposer
{
    public bool IsPlaying { get; init; }
}