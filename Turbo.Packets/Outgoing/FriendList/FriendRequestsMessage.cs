using System.Collections.Generic;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record FriendRequestsMessage : IComposer
{
    public required List<IMessengerRequest> Requests { get; init; }
}
