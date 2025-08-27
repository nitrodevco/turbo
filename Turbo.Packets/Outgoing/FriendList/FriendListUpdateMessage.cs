using System.Collections.Generic;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;
public record FriendListUpdateMessage : IComposer
{
    public required IReadOnlyDictionary<MessengerUpdateTypeEnum, IReadOnlyList<IMessengerUpdateItem>> FriendListUpdate { get; init; }
}
