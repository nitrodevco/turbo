using Turbo.Core.Game.Messenger;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record NewFriendRequestMessage : IComposer
{
    public required IMessengerRequest Request { get; init; }
}
