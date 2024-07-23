using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Competition;

public class CurrentTimingCodeMessage : IComposer
{
    public string SchedulingStr { get; init; }
    public string Code { get; init; }
}