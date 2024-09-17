using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class GuildBaseSearchMessage : IMessageEvent
{
    public int Unknown { get; init; }
}