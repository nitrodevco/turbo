using Turbo.Core.Game.Furniture;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Furni
{
    public record FurniListAddOrUpdateMessage : IComposer
    {
        public IPlayerFurniture Furniture { get; init; }
    }
}
