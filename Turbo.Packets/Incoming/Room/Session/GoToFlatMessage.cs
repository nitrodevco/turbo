using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Session;

public record GoToFlatMessage : IMessageEvent
{
    public int RoomId { get; init; }
}