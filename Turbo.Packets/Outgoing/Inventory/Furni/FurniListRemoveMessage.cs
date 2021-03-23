namespace Turbo.Packets.Outgoing.Inventory.Furni
{
    public record FurniListRemoveMessage
    {
        public int ItemId { get; init; }
    }
}
