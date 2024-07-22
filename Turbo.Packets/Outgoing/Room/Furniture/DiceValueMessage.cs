using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record DiceValueMessage : IComposer
{
    public int ItemId { get; init; }
    public int Value { get; init; }
}