using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Tracking;

public class LatencyPingRequestMessage : IMessageEvent
{
    public int ID { get; init; }
}