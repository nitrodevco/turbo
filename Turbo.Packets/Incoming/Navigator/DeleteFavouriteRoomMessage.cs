using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class DeleteFavouriteRoomMessage : IMessageEvent
{
    public int RoomId { get; init; }
}