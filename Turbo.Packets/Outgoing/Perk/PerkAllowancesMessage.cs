using System.Collections.Generic;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Perk;

public record PerkAllowancesMessage : IComposer
{
    public IList<PlayerPerkEnum>? PlayerPerks { get; init; }
}