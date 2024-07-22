using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action;

public record UnbanUserFromRoomMessage : IMessageEvent
{
    public int PlayerId { get; init; }
    public int RoomId { get; init; }
}