using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Purse;
public record CreditBalanceMessage : IComposer
{
    public int credits { get; init; }
}
