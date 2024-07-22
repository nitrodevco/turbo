using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Preferences;

public record ChatStylePreferenceMessage : IMessageEvent
{
    public int StyleId { get; init; }
}