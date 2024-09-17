using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class ConvertGlobalRoomIdMessage : IMessageEvent
{
    public string FlatId { get; init; }
}