using System;
using System.Collections.Generic;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Inventory
{
    public interface IPlayerBadgeInventory : IAsyncInitialisable, IAsyncDisposable
    {
        public IDictionary<string, IPlayerBadge> Badges { get; }
        public IList<IPlayerBadge> ActiveBadges { get; }
        public void ResetActiveBadges();
        public void SetActivedBadges(IDictionary<int, string> badges);
        public void SendBadgesToSession(ISession session);
    }
}