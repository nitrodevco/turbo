using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class AddFavouriteRoomMessage : IMessageEvent
{
    public int RoomId { get; init; }
}