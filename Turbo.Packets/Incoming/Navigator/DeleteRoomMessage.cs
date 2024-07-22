using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record DeleteRoomMessage : IMessageEvent
{
    public int RoomId { get; init; }
}