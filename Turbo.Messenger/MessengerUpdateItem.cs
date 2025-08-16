using Turbo.Core.Game.Messenger;

namespace Turbo.Messenger;
public readonly record struct MessengerUpdateItem : IMessengerUpdateItem
{
    public IMessengerFriend? Friend { get; }
    public int? FriendId { get; }
    public bool IsFriend => Friend is not null;
    public bool IsId => FriendId.HasValue;

    private MessengerUpdateItem(IMessengerFriend? friend, int? friendId)
    {
        Friend = friend;
        FriendId = friendId;
    }

    public static MessengerUpdateItem FromFriend(IMessengerFriend friend) => new(friend, null);
    public static MessengerUpdateItem FromId(int id) => new(null, id);
}