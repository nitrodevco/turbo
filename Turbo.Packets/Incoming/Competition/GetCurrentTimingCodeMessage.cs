using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Competition;

public class GetCurrentTimingCodeMessage : IMessageEvent
{
    public string TimingCode { get; init; }
}