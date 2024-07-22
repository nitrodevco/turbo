using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Furni;

public record FurniListRemoveMessage : IComposer
{
    public int ItemId { get; init; }
}