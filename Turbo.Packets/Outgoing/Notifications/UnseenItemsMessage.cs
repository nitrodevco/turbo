using System.Collections.Generic;
using Turbo.Core.Game.Inventory.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Notifications;

public class UnseenItemsMessage : IComposer
{
    public IDictionary<UnseenItemCategory, IList<int>> Categories { get; init; }
}