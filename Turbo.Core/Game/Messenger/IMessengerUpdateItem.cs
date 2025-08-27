namespace Turbo.Core.Game.Messenger;

public interface IMessengerUpdateItem
{
    IMessengerFriend? Friend { get; }
    int? FriendId { get; }
    bool IsFriend { get; }
    bool IsId { get; }
}
