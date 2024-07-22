using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Permissions;

public record YouAreControllerMessage : IComposer
{
    public int RoomId { get; set; }
    public RoomControllerLevel RoomControllerLevel { get; set; }
}