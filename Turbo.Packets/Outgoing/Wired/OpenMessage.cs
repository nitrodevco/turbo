using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Wired;

public record OpenMessage : IComposer
{
    public int ItemId { get; init; }
}