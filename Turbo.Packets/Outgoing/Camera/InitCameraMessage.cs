using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Camera;

public record InitCameraMessage : IComposer
{
    public required int CostCredits { get; init; }
    public required int CostCurrency { get; init; }
    public required int CurrencyType { get; init; }
}
