using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class ForwardToARandomPromotedRoomMessage : IMessageEvent
{
    public string Category { get; init; }
}