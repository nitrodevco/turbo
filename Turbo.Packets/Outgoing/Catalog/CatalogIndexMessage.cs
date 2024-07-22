using Turbo.Core.Game.Catalog;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog;

public record CatalogIndexMessage : IComposer
{
    public ICatalogPage Root { get; init; }
    public bool NewAdditionsAvailable { get; init; }
    public string CatalogType { get; init; }
}