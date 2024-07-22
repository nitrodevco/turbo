using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action;

public record MuteUserMessage : IMessageEvent
{
    public int PlayerId { get; init; }
    public int RoomId { get; init; }
    public int Minutes { get; init; }
}