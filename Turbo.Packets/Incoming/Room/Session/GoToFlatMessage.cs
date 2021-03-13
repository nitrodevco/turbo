using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Pets
{
    public record GoToFlatMessage : IMessageEvent
    {
        public int RoomId { get; init; }
    }
}
