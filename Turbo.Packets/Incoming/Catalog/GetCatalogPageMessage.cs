using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record GetCatalogPageMessage : IMessageEvent
{
    public int PageId { get; init; }
    public int OfferId { get; init; }
    public string Type { get; init; }
}