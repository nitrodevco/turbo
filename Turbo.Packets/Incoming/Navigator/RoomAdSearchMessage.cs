using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class RoomAdSearchMessage : IMessageEvent
{
    public int Unknown1 { get; init; }
    public int Unknown2 { get; init; }
}