using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Furni;

public record PostItPlacedMessage : IComposer
{
    public int Id { get; init; }
    public int ItemsLeft { get; init; }
}