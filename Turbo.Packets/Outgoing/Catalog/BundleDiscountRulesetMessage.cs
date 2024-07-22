using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog;

public record BundleDiscountRulesetMessage : IComposer
{
    public int MaxPurchaseSize { get; init; }
    public int BundleSize { get; init; }
    public int BundleDiscountSize { get; init; }
    public int BonusThreshold { get; init; }
    public int[] AdditionalBonusDiscountThresholdQuantities { get; init; }
}