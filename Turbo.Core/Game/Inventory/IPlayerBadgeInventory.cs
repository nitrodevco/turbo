using System.Collections.Concurrent;
using System.Collections.Generic;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Inventory;

public interface IPlayerBadgeInventory : IComponent
{
    public ConcurrentDictionary<string, IPlayerBadge> Badges { get; }
    public IList<IPlayerBadge> ActiveBadges { get; }
    public void ResetActiveBadges();
    public void SetActivedBadges(IDictionary<int, string> badges);
    public void SendBadgesToSession(ISession session);
}