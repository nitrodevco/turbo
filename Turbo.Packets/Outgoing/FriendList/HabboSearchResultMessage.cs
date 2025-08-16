using System.Collections.Generic;
using Turbo.Core.Game.Players;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record HabboSearchResultMessage : IComposer
{
    public List<IPlayer> Friends { get; init; }
    public List<IPlayer> Others { get; init; }
}