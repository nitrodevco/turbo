using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record BuildersClubPlaceWallItemMessage : IMessageEvent
{
    public int PageId { get; init; }
    public int OfferId { get; init; }
    public string ExtraParam { get; init; }
    public string Location { get; init; }
}