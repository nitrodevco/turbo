using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record GetCatalogIndexMessage : IMessageEvent
{
    public string Type { get; init; }
}