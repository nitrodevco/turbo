using Turbo.Core.Game.Players;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Users;
public record ExtendedProfileMessage : IComposer
{
    public required IPlayer Player { get; init; }
}