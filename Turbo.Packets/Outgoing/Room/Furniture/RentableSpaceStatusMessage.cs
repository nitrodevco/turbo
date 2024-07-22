using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record RentableSpaceStatusMessage : IComposer
{
    public bool IsRented { get; init; }
    public int ErrorCode { get; init; }
    public int RenterId { get; init; }
    public string RenterName { get; init; }
    public int ExpiryTime { get; init; }
    public int RentCost { get; init; }
}