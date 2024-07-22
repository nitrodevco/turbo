using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record ItemDataUpdateMessage : IComposer
{
    public int ItemId { get; init; }
    public string ItemData { get; init; }
}