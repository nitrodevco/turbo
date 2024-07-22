using Turbo.Core.Game.Inventory;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Badges;

public class BadgeReceivedMessage : IComposer
{
    public IPlayerBadge Badge { get; init; }
}