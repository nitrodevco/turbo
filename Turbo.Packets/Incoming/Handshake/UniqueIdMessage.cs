using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Handshake;

public record UniqueIdMessage : IMessageEvent
{
    public string MachineID { get; init; }
    public string Fingerprint { get; init; }
    public string FlashVersion { get; init; }
}