using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Avatar;

public record LookToMessage : IMessageEvent
{
    public int LocX { get; init; }
    public int LocY { get; init; }
}