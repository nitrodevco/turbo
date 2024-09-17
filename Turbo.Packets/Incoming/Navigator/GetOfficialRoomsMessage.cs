using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class GetOfficialRoomsMessage : IMessageEvent
{
    public int AdIndex { get; init; }
}