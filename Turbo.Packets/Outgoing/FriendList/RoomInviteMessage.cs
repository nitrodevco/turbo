using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record RoomInviteMessage : IComposer
{
    public int SenderId { get; init; }
    public string Message { get; init; }
}
