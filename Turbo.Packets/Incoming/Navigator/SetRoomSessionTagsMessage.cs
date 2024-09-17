using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class SetRoomSessionTagsMessage : IMessageEvent
{
    public string Tag1 { get; init; }
    public string Tag2 { get; init; }
}