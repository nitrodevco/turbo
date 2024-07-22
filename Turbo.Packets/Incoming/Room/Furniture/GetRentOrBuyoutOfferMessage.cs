using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record GetRentOrBuyoutOfferMessage : IMessageEvent
{
    public bool IsWallFurniture { get; init; }
    public string ItemName { get; init; }
    public bool IsBuyout { get; init; }
}