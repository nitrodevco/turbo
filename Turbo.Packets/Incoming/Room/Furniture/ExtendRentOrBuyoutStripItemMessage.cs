using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record ExtendRentOrBuyoutStripItemMessage : IMessageEvent
{
    public int StripId { get; init; }
    public bool IsBuyout { get; init; }
}