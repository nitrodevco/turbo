using System.Collections.Generic;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Badges;

public class BadgeMessage : IComposer
{
    public IList<IPlayerBadge> Badges { get; init; }
    public IList<IPlayerBadge> ActiveBadges { get; init; }
}