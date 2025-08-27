using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Messenger;
public interface IMessenger : IComponent
{
    ILogger<IMessenger> Logger { get; }
    int Id { get; }
    IPlayer Player { get; }
    ConcurrentDictionary<int, IMessengerCategory> Categories { get; }
    ConcurrentDictionary<int, IMessengerFriend> Friends { get; }
    ConcurrentDictionary<int, IMessengerRequest> Requests { get; }
    bool ClientInitialized { get; set; }
    Task SetClientInitialized();
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
    void SendUpdateToFriends(bool commit = false);
    void UpdateFriendRelation(int friendId, MessengerFriendRelationEnum relationType);
    IReadOnlyDictionary<MessengerUpdateTypeEnum, IReadOnlyList<IMessengerUpdateItem>> GetAndClearUpdates();
    Task<IReadOnlyList<IMessengerConsoleMessage>> GetConsoleHistory(int friendId, string? messageId);
    Task SendMessage(int friendId, string message, int confirmationId);
    void ReceiveMessage(IMessengerConsoleMessage message, bool queueMessage = false);
    void SendRoomInvite(string message, int[] friendIds);
}