using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action
{
    public record RoomUserKickMessage : IMessageEvent
    {
        public int PlayerId { get; init; }
    }
}
