using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Tracking;

// TODO: properties are not correctly named.
public record EventLogMessage : IMessageEvent
{
    public string Event { get; init; }
    public string Data { get; init; }
    public string Action { get; init; }
    public string ExtraString { get; init; }
    public int ExtraInt { get; init; }
}