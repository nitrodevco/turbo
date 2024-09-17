using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class RateFlatMessage : IMessageEvent
{
    public int Points { get; init; }
}