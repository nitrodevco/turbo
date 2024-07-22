using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record PresentOpenedMessage : IComposer
{
    public string ItemType { get; init; }
    public int SpriteId { get; init; }
    public string ProductCode { get; init; }
    public int PlacedItemId { get; init; }
    public string PlacedItemType { get; init; }
    public bool PlacedInRoom { get; init; }
    public string PetFigureString { get; init; }
}