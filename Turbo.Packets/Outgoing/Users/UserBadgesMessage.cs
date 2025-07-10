using System.Collections.Generic;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Users;

public record UserBadgesMessage : IComposer
{
    public int PlayerId { get; init; }
    public required IList<IPlayerBadge> ActiveBadges { get; init; }
}