using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Packets.Outgoing.Inventory.Furni
{
    public record FurniListAddOrUpdateMessage
    {
        public IList<IRoomObject> Furniture { get; init; }
    }
}
