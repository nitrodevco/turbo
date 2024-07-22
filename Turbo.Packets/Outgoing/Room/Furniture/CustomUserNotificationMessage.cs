using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record CustomUserNotificationMessage : IComposer
{
    public int Code { get; init; }
}