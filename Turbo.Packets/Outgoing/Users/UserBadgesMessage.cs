using System.Collections.Generic;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Game.Inventory;

namespace Turbo.Packets.Outgoing.Users
{
    public record UserBadgesMessage : IComposer
    {
        public int PlayerId { get; init; }
        public IList<IPlayerBadge> ActiveBadges { get; init; }
    }
}