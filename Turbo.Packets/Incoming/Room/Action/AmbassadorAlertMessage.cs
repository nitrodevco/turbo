using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action
{
    public record AmbassadorAlertMessage : IMessageEvent
    {
        public int UserId { get; init; }
    }
}
