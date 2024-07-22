using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record SelectClubGiftMessage : IMessageEvent
{
    public string ProductCode { get; init; }
}