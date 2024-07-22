using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog;

public record PurchaseNotAllowedMessage : IComposer
{
    public PurchaseNotAllowedEnum ErrorCode { get; init; }
}