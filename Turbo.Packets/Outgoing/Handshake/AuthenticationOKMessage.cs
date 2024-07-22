using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake;

public record AuthenticationOKMessage : IComposer
{
    public int? AccountId { get; set; }
    public short[]? SuggestedLoginActions { get; set; }
    public int? IdentityId { get; set; }
}