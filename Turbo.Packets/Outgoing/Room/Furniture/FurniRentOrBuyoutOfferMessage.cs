using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record FurniRentOrBuyoutOfferMessage : IComposer
{
    public bool IsWallItem { get; init; }
    public string FurniTypeName { get; init; }
    public bool Buyout { get; init; }
    public int PriceInCredits { get; init; }
    public int PriceInActivityPoints { get; init; }
    public int ActivityPointsType { get; init; }
}