using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record CustomStackingHeightUpdateMessage : IComposer
{
    public int ItemId { get; init; }
    public int Height { get; init; }
}