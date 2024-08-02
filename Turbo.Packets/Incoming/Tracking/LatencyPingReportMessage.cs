using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Tracking;

public class LatencyPingReportMessage : IMessageEvent
{
    public int AverageLatency { get; init; }
    public int ValidPingAverage { get; init; }
    public int NumPings { get; init; }
}