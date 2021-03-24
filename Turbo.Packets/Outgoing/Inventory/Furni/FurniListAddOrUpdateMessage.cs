using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Furni
{
    public record FurniListAddOrUpdateMessage : IComposer
    {
        public IList<IRoomObject> Furniture { get; init; }
    }
}
