using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record ObjectRemoveMessage : IComposer
{
    public int Id { get; init; }
    public bool IsExpired { get; init; }
    public int PickerId { get; init; }
    public int Delay { get; init; }
}