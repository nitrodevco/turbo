using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Wired;

public record ApplySnapshotMessage : IMessageEvent
{
    public int ItemId { get; init; }
}