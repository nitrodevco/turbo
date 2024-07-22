using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record MoveAvatarMessage : IMessageEvent
{
    public int X { get; init; }
    public int Y { get; init; }
}