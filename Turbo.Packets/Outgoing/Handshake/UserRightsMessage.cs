using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake;

public record UserRightsMessage : IComposer
{
    public ClubLevelEnum ClubLevel { get; init; }
    public SecurityLevelEnum SecurityLevel { get; init; }
    public bool IsAmbassador { get; init; }
}