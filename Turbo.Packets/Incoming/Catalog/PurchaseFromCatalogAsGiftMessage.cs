using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record PurchaseFromCatalogAsGiftMessage : IMessageEvent
{
    public int PageId { get; init; }
    public int OfferCode { get; init; }
    public string ExtraParam { get; init; }
    public string RecieverName { get; init; }
    public string Message { get; init; }
    public int BoxStuffTypeId { get; init; }
    public int BoxTypeId { get; init; }
    public int RibbonTypeId { get; init; }
    public bool ShowPurchaserName { get; init; }
}