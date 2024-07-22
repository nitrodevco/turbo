using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record GetGuestRoomMessage : IMessageEvent
{
    public int RoomId { get; init; }
    public bool EnterRoom { get; init; }
    public bool RoomForward { get; init; }
}