using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record InstantMessageErrorMessage : IComposer
{
    public int ErrorCode { get; init; }
    public int PlayerId { get; init; }
    public string Message { get; init; }
}
