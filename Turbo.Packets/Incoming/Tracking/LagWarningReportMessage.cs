using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Tracking;

public class LagWarningReportMessage : IMessageEvent
{
    public int WarningCount { get; init; }
}