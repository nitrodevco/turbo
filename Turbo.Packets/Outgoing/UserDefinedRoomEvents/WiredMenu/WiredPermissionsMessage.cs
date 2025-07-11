using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.UserDefinedRoomEvents.WiredMenu;

public record WiredPermissionsMessage : IComposer
{
    public bool CanModify { get; init; }
    public bool CanRead { get; init; }
}