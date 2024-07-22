using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake;

public record UniqueMachineIdMessage : IComposer
{
    public string MachineID { get; init; }
}