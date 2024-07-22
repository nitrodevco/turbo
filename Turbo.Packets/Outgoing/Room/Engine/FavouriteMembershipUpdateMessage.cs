using Turbo.Core.Game.Groups;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record FavouriteMembershipUpdateMessage : IComposer
{
    public int RoomIndex { get; init; }
    public int GroupId { get; init; }
    public GroupState Status { get; init; }
    public string GroupName { get; init; }
}