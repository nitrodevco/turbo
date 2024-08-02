using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Tracking;

public class LatencyPingResponseMessage : IComposer
{
    public int ID { get; set; }
}