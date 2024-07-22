using System.Collections.Generic;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Furni;

public record FurniListMessage : IComposer
{
    public int TotalFragments { get; init; }
    public int CurrentFragment { get; init; }
    public IList<IPlayerFurniture> Furniture { get; init; }
}