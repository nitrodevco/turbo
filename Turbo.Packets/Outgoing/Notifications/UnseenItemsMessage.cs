using Turbo.Core.Packets.Messages;
using System.Collections.Generic;
using Turbo.Core.Game.Inventory.Constants;

namespace Turbo.Packets.Outgoing.Notifications
{
    public class UnseenItemsMessage : IComposer
    {
        public IDictionary<UnseenItemCategory, IList<int>> Categories { get; init; }
    }
}