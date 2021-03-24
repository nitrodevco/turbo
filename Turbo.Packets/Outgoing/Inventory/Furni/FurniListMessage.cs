using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Furni
{
    public record FurniListMessage : IComposer
    {
        public int TotalFragments { get; init; }
        public int CurrentFragment { get; init; }
        public IList<IRoomObject> Furniture { get; init; }
    }
}
