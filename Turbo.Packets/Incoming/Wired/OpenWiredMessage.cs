using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Wired;

public record OpenWiredMessage : IMessageEvent
{
    public int ItemId { get; init; }
}