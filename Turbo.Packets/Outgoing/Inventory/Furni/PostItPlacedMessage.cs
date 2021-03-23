namespace Turbo.Packets.Outgoing.Inventory.Furni
{
    public record PostItPlacedMessage
    {
        public int Id { get; init; }
        public int ItemsLeft { get; init; }
    }
}
