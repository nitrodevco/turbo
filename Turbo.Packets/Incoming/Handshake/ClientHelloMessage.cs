using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Handshake;

public record ClientHelloMessage : IMessageEvent
{
    public string Production { get; init; }
    public string Platform { get; init; }
    public int ClientPlatform { get; init; }
    public int DeviceCategory { get; init; }
}