using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Messenger;

namespace Turbo.Core.Game.Messenger;

public interface IMessenger : IComponent
{
    ILogger<IMessenger> Logger { get; }
    int Id { get; }
    IPlayer Player { get; }
    ConcurrentDictionary<int, IMessengerCategory> Categories { get; }
    ConcurrentDictionary<int, IMessengerFriend> Friends { get; }
    ConcurrentDictionary<int, IMessengerRequest> Requests { get; }
    IMessengerFriend? GetFriend(int playerId);
    int GetFriendsCount();
    List<List<IMessengerFriend>> GetFriendsFragments(int fragmentSize);
    void AddFriend(IMessengerFriend messengerFriend);
    Task AcceptFriends(int[] playerIds);
    void RemoveFriend(int friendId);
    Task RemoveFriends(int[] friendIds);
    IMessengerRequest? GetRequest(int playerId);
    int GetRequestsCount();
    void AddRequest(IMessengerRequest messengerRequest);
    Task<IMessengerRequest?> SendRequest(IPlayer targetPlayer);
    Task DeleteRequest(int playerId);
    Task DeleteRequests(int[] playerIds);
    Task ClearRequests();
    bool HasPendingRequest(int playerId);
    void UpdateFriend(IPlayer player, bool commit = true);
    Task SendUpdateToFriends(bool commit = false);
    void UpdateFriendRelation(int friendId, MessengerFriendRelationEnum relationType);
    IReadOnlyDictionary<MessengerUpdateTypeEnum, IReadOnlyList<IMessengerUpdateItem>> GetAndClearUpdates();
    void SendMessage(int friendId, string message);
    void SendRoomInvite(string message, int[] friendIds);
}
