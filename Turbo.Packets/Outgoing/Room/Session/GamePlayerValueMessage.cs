using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record GamePlayerValueMessage : IComposer
{
    public int UserId { get; init; }
    public int Value { get; init; }
}