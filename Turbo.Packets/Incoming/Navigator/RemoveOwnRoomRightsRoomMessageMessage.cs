using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class RemoveOwnRoomRightsRoomMessageMessage : IMessageEvent
{
    public int RoomId { get; init; }
}