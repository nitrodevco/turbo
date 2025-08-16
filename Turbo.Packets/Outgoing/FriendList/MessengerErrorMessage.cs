using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record MessengerErrorMessage : IComposer
{
    public MessengerErrorEnum ErrorCode { get; init; }
}
