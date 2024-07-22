using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Handshake;

public record VersionCheckMessage : IMessageEvent
{
    public int ClientID { get; init; }
    public string ClientURL { get; init; }
    public string ExternalVariablesURL { get; init; }
}