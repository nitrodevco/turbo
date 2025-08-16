using System.Collections.Concurrent;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Messenger.Constants;

namespace Turbo.Messenger;

public class MessengerUpdate : IMessengerUpdate
{
    private readonly ConcurrentDictionary<MessengerUpdateTypeEnum, List<IMessengerUpdateItem>> _updates = new();

    public int UpdatesCount => _updates.Values.Sum(list => list.Count);

    public IReadOnlyDictionary<MessengerUpdateTypeEnum, IReadOnlyList<IMessengerUpdateItem>> Items =>
        _updates.ToDictionary(kv => kv.Key, kv => (IReadOnlyList<IMessengerUpdateItem>)kv.Value);

    public void AddAdded(IMessengerFriend friend) =>
        AddInternal(MessengerUpdateTypeEnum.Added, MessengerUpdateItem.FromFriend(friend));

    public void AddUpdated(IMessengerFriend friend) =>
        AddInternal(MessengerUpdateTypeEnum.Updated, MessengerUpdateItem.FromFriend(friend));

    public void AddRemoved(int friendId) =>
        AddInternal(MessengerUpdateTypeEnum.Removed, MessengerUpdateItem.FromId(friendId));

    public bool HasUpdates => UpdatesCount > 0;

    public void Clear() => _updates.Clear();

    private void AddInternal(MessengerUpdateTypeEnum type, IMessengerUpdateItem item)
    {
        var list = _updates.GetOrAdd(type, static _ => new List<IMessengerUpdateItem>());
        lock (list)
        {
            list.Add(item);
        }
    }
}