using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record ChargeFireworkMessage : IMessageEvent
{
    public int SpriteId { get; init; }
    public int Type { get; init; }
}