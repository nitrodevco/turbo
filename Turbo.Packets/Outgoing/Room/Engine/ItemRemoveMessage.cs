using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record ItemRemoveMessage : IComposer
{
    public int ItemId { get; init; }
    public int PickerId { get; init; }
}