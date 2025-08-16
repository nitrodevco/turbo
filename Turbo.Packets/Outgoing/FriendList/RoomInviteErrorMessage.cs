using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record RoomInviteErrorMessage : IComposer
{
    public int ErrorCode { get; init; }
    public List<int>? FailedRecipients { get; init; }
}
