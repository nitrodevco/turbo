using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog;

public record BuildersClubFurniCountMessage : IComposer
{
    public int FurniCount { get; init; }
}