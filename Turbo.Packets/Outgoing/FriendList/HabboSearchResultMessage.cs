using System.Collections.Generic;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Players;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record HabboSearchResultMessage : IComposer
{
    public List<IMessengerSearchResult> Friends { get; init; }
    public List<IMessengerSearchResult> Others { get; init; }
}