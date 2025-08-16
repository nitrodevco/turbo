using Turbo.Core.Game.Messenger;

namespace Turbo.Messenger;

public interface IMessengerUpdateItem
{
    IMessengerFriend? Friend { get; }
    int? FriendId { get; }
    bool IsFriend { get; }
    bool IsId { get; }
}