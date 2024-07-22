using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action;

public record SleepMessage : IComposer
{
    public int ObjectId { get; init; }
    public bool Sleeping { get; init; }
}