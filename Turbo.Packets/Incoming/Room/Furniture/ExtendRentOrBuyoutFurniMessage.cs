using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record ExtendRentOrBuyoutFurniMessage : IMessageEvent
{
    public bool IsWallFurniture { get; init; }
    public int RoomId { get; init; }
    public bool IsBuyout { get; init; }
}