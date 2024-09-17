using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class UpdateHomeRoomMessage : IMessageEvent
{
    public int RoomId { get; init; }
}