using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.RoomSettings;

public record GetBannedUsersFromRoomMessage : IMessageEvent
{
    public int RoomId { get; init; }
}