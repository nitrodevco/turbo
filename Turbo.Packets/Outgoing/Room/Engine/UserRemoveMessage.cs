using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record UserRemoveMessage : IComposer
{
    public int Id { get; init; }
}