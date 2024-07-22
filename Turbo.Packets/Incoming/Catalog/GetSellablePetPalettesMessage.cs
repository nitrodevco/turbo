using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record GetSellablePetPalettesMessage : IMessageEvent
{
    public int LocalizationId { get; init; }
}