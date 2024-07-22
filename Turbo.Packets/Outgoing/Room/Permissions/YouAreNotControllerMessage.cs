using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Permissions;

public record YouAreNotControllerMessage : IComposer
{
    public int RoomId { get; set; }
}