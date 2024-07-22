using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Bots;

public record BotForceOpenContextMenuMessage : IComposer
{
    public int BotId { get; init; }
}