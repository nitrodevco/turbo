using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Wired;

public record WiredRewardResultMessage : IComposer
{
    public int Reason { get; init; }
}