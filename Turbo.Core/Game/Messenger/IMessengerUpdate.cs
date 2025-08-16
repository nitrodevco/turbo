using System.Collections.Generic;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Messenger;

namespace Turbo.Core.Game.Messenger;

public interface IMessengerUpdate
{
    int UpdatesCount { get; }
    bool HasUpdates { get; }
    IReadOnlyDictionary<MessengerUpdateTypeEnum, IReadOnlyList<IMessengerUpdateItem>> Items { get; }

    void AddAdded(IMessengerFriend friend);
    void AddUpdated(IMessengerFriend friend);
    void AddRemoved(int friendId);
    void Clear();
}