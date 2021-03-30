using System.Collections.Generic;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Security;

namespace Turbo.Packets.Outgoing.Perks
{
    public record PerkAllowancesMessage : IComposer
    {
        public IList<Perk> Perks { get; init; }
    }
}
