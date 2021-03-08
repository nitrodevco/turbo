using Turbo.Core.Navigator.Enums;

namespace Turbo.Packets.Incoming.Navigator
{
    public record ForwardToSomeRoomMessage : IMessageEvent
    {
        public NavigatorRoomForwardType ForwardType { get; init; }
    }
}
