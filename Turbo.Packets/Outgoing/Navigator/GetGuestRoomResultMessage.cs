using Turbo.Core.Game.Rooms;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record GetGuestRoomResultMessage : IComposer
{
    public bool EnterRoom { get; init; }
    public IRoom Room { get; init; }
    public bool IsRoomForward { get; init; }
    public bool IsStaffPick { get; init; }
    public bool IsGroupMember { get; init; }
    public bool AllInRoomMuted { get; init; }
    public bool CanMute { get; init; }
}