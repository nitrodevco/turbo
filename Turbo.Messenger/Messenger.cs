using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Logs.Chat;
using Turbo.Database.Entities.Messenger;
using Turbo.Database.Repositories.Logs.Chat;
using Turbo.Database.Repositories.Messenger;
using Turbo.Packets.Incoming.Room.Chat;
using Turbo.Packets.Outgoing.FriendList;

namespace Turbo.Messenger;

public class Messenger(
    ILogger<IMessenger> _logger,
    IServiceScopeFactory _scopeFactory,
    IPlayer _player) : Component, IMessenger
{
    public ILogger<IMessenger> Logger => _logger;
    public IPlayer Player => _player;
    public int Id => Player.Id;

    private readonly ConcurrentDictionary<int, IMessengerCategory> _categories = new();
    private readonly ConcurrentDictionary<int, IMessengerFriend> _friends = new();
    private readonly ConcurrentDictionary<int, IMessengerRequest> _requests = new();
    private readonly IMessengerUpdate _updates = new MessengerUpdate();

    private readonly ConcurrentDictionary<int, IMessengerConsoleMessage> _pendingConsoleMessages = new();
    private ConcurrentDictionary<int, bool> _historySentForFriend => new();

    public ConcurrentDictionary<int, IMessengerCategory> Categories => _categories;
    public ConcurrentDictionary<int, IMessengerFriend> Friends => _friends;
    public ConcurrentDictionary<int, IMessengerRequest> Requests => _requests;
    public bool ClientInitialized { get; set; }
    protected override async Task OnInit()
    {
        try
        {
            await LoadFriends();
            await LoadRequests();
            await LoadPendingMessages();
            ClientInitialized = false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to initialize Messenger for player {PlayerId}", Player.Id);
            throw;
        }
    }

    private async Task LoadFriends()
    {
        using var scope = _scopeFactory.CreateScope();
        var messengerFriendsRepository = scope.ServiceProvider.GetRequiredService<IMessengerFriendsRepository>();
        var messengerFriendEntities = await messengerFriendsRepository.FindAllFriends(Id);

        Friends.Clear();

        if (messengerFriendEntities is not null)
        {
            foreach (var friendEntity in messengerFriendEntities)
            {
                var friend = new MessengerFriend(friendEntity);
                Friends.TryAdd(friend.Id, friend);
            }
        }
    }

    private async Task LoadRequests()
    {
        using var scope = _scopeFactory.CreateScope();
        var messengerRequestRepository = scope.ServiceProvider.GetRequiredService<IMessengerRequestsRepository>();
        var messengerRequestEntities = await messengerRequestRepository.FindAllFriendRequests(Id);

        Requests.Clear();

        if (messengerRequestEntities is not null)
        {
            foreach (var requestEntity in messengerRequestEntities)
            {
                var request = new MessengerRequest(requestEntity);
                Requests.TryAdd(request.Id, request);
            }
        }
    }

    private async Task LoadPendingMessages()
    {
        using var scope = _scopeFactory.CreateScope();
        var consoleChatLogsRepository = scope.ServiceProvider.GetRequiredService<IConsoleChatLogsRepository>();
        var pendingMessages = await consoleChatLogsRepository.GetPendingConsoleChatLogs(Id);

        _pendingConsoleMessages.Clear();

        if (pendingMessages is null || pendingMessages.Count == 0) return;

        var now = DateTime.UtcNow;

        foreach (var log in pendingMessages)
        {
            var chatId = log.SenderEntityId == Id ? log.RecipientEntityId : log.SenderEntityId;
            var secondsSinceSent = (int)(now - log.SentAt).TotalSeconds;
            var confirmationId = log.SenderEntityId == Id ? log.ConfirmationId : 0;
            var message = new MessengerConsoleMessage(chatId, secondsSinceSent, log, confirmationId);
            _pendingConsoleMessages.TryAdd(message.Id, message);
        }
    }

    private async Task LoadCategories() => throw new NotImplementedException("Loading categories is not implemented yet.");

    public async Task SetClientInitialized()
    {
        if (ClientInitialized) return;

        ClientInitialized = true;

        foreach (var friend in Friends.Values)
        {
            var liveFriend = Player.PlayerManager.GetPlayerById(friend.Id);
            if (liveFriend is not null && liveFriend.IsInitialized)
            {
                UpdateFriend(liveFriend, false);
            }
        }

        SendUpdateToFriends(false);
        CommitUpdates();
        await SendPendingMessages();
    }

    public IMessengerFriend? GetFriend(int playerId) => Friends.TryGetValue(playerId, out var friend) ? friend : null;

    public int GetFriendsCount() => Friends.Count;

    public List<List<IMessengerFriend>> GetFriendsFragments(int fragmentSize)
    {
        if (fragmentSize <= 0) throw new ArgumentOutOfRangeException(nameof(fragmentSize));
        var snapshot = Friends.Values.ToArray();
        if (snapshot.Length == 0) return new() { new List<IMessengerFriend>() };
        return snapshot
            .Chunk(fragmentSize)
            .Select(chunk => new List<IMessengerFriend>(chunk))
            .ToList();
    }

    public void AddFriend(IMessengerFriend messengerFriend)
    {
        if (messengerFriend is null || messengerFriend.Id <= 0) return;
        Friends.TryAdd(messengerFriend.Id, messengerFriend);
        _updates.AddAdded(messengerFriend);
        CommitUpdates();
    }

    public async Task AcceptFriends(int[] playerIds)
    {
        if (playerIds is null || playerIds.Length == 0) return;

        foreach (int playerId in playerIds)
        {
            if (playerId == Id) continue;

            if (HasPendingRequest(playerId))
            {
                await DeleteRequest(playerId);

                using var scope = _scopeFactory.CreateScope();
                var messengerFriendsRepository = scope.ServiceProvider.GetRequiredService<IMessengerFriendsRepository>();
                var (friendBuddyEntity, meAsBuddyEntity) = await messengerFriendsRepository.CreateFriendship(Id, playerId);

                if (friendBuddyEntity is null || meAsBuddyEntity is null) continue;

                var friendBuddy = new MessengerFriend(friendBuddyEntity);
                AddFriend(friendBuddy);

                var friend = Player.PlayerManager.GetPlayerById(friendBuddy.Id);

                if (friend is not null && friend.PlayerDetails.PlayerStatus is PlayerStatusEnum.Online)
                {
                    var meAsBuddy = new MessengerFriend(meAsBuddyEntity);
                    friend.Messenger.AddFriend(meAsBuddy);
                }
            }
        }
    }

    public void RemoveFriend(int friendId)
    {
        if (friendId <= 0) return;
        Friends.TryRemove(friendId, out _);
        _updates.AddRemoved(friendId);
        CommitUpdates();
    }

    public async Task RemoveFriends(int[] friendIds)
    {
        if (friendIds is null || friendIds.Length == 0) return;
        IEnumerable<int> validFriendIds = friendIds.Where(id => id > 0 && Friends.ContainsKey(id));

        using var scope = _scopeFactory.CreateScope();
        var messengerFriendsRepository = scope.ServiceProvider.GetRequiredService<IMessengerFriendsRepository>();
        await messengerFriendsRepository.DeleteFriendships(Id, validFriendIds);

        foreach (int friendId in validFriendIds)
        {
            Friends.TryRemove(friendId, out _);
            _updates.AddRemoved(friendId);

            var exFriend = Player.PlayerManager.GetPlayerById(friendId);

            if (exFriend is not null && exFriend.PlayerDetails.PlayerStatus is PlayerStatusEnum.Online)
            {
                exFriend.Messenger.RemoveFriend(Id);
            }
        }

        CommitUpdates();
    }

    public void UpdateFriend(IPlayer player, bool commit = true)
    {
        if (player is null || player.Id <= 0) return;

        var friend = GetFriend(player.Id);

        if (friend is null) return;

        friend.Name = player.Name;
        friend.Motto = player.PlayerDetails.Motto;
        friend.Figure = player.PlayerDetails.Figure;
        friend.Status = player.PlayerDetails.PlayerStatus;

        var friendIsInRoom = player.RoomObject is not null;
        friend.CanBeFollowed = friendIsInRoom;

        _updates.AddUpdated(friend);

        if (commit)
        {
            CommitUpdates();
        }
    }

    public void SendUpdateToFriends(bool commit = false)
    {
        foreach (var friend in Friends.Values)
        {
            if (friend is null || friend.Status is PlayerStatusEnum.Offline) continue;

            var player = Player.PlayerManager.GetPlayerById(friend.Id);

            if (player is not null && player.IsInitialized)
            {
                player.Messenger.UpdateFriend(Player, commit);
            }
        }

        if(commit)
        {
            CommitUpdates();
        }
    }

    public void UpdateFriendRelation(int friendId, MessengerFriendRelationEnum relationType) => throw new NotImplementedException();

    private void CommitUpdates()
    {
        if (!IsInitialized || IsDisposing || IsDisposed || !_updates.HasUpdates || !ClientInitialized) return;

        Player.Session.Send(new FriendListUpdateMessage
        {
            FriendListUpdate = GetAndClearUpdates()
        });
    }

    public IReadOnlyDictionary<MessengerUpdateTypeEnum, IReadOnlyList<IMessengerUpdateItem>> GetAndClearUpdates()
    {
        if (!_updates.HasUpdates)
            return new Dictionary<MessengerUpdateTypeEnum, IReadOnlyList<IMessengerUpdateItem>>();

        var snapshot = _updates.Items.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<IMessengerUpdateItem>)kv.Value.ToList());

        _updates.Clear();
        return snapshot;
    }

    public IMessengerRequest? GetRequest(int playerId) => Requests.TryGetValue(playerId, out var request) ? request : null;

    public int GetRequestsCount() => Requests.Count;

    public void AddRequest(IMessengerRequest messengerRequest) => Requests.TryAdd(messengerRequest.Id, messengerRequest);

    public async Task<IMessengerRequest?> SendRequest(IPlayer targetPlayer)
    {
        if (targetPlayer.Id <= 0 || HasPendingRequest(targetPlayer.Id) || GetFriend(targetPlayer.Id) is not null) return null;

        using var scope = _scopeFactory.CreateScope();
        var messengerRequestsRepository = scope.ServiceProvider.GetRequiredService<IMessengerRequestsRepository>();
        var requestEntity = await messengerRequestsRepository.CreateFriendRequest(Id, targetPlayer.Id);

        return new MessengerRequest(requestEntity);
    }

    public async Task DeleteRequest(int playerId)
    {
        if (playerId <= 0 || !HasPendingRequest(playerId)) return;

        Requests.TryRemove(playerId, out _);

        using var scope = _scopeFactory.CreateScope();
        var messengerRequestsRepository = scope.ServiceProvider.GetRequiredService<IMessengerRequestsRepository>();
        await messengerRequestsRepository.DeleteFriendRequest(playerId, Id);
    }

    public async Task DeleteRequests(int[] playerIds)
    {
        if (playerIds is null || playerIds.Length == 0) return;
        IEnumerable<int> validPlayerIds = playerIds.Where(id => id > 0 && HasPendingRequest(id));

        using var scope = _scopeFactory.CreateScope();
        var messengerRequestsRepository = scope.ServiceProvider.GetRequiredService<IMessengerRequestsRepository>();
        await messengerRequestsRepository.DeleteFriendRequests(validPlayerIds, Id);

        foreach (int playerId in validPlayerIds)
        {
            Requests.TryRemove(playerId, out _);
        }
    }

    public async Task ClearRequests()
    {
        if (Requests.IsEmpty) return;

        using var scope = _scopeFactory.CreateScope();
        var messengerRequestsRepository = scope.ServiceProvider.GetRequiredService<IMessengerRequestsRepository>();
        await messengerRequestsRepository.ClearFriendRequests(Id);

        Requests.Clear();
    }

    public bool HasPendingRequest(int playerId) => Requests.ContainsKey(playerId);

    public async Task<IReadOnlyList<IMessengerConsoleMessage>> GetConsoleHistory(int friendId, string? messageId)
    {
        if (friendId <= 0 || friendId == Id) return Array.Empty<IMessengerConsoleMessage>();

        var friend = GetFriend(friendId);
        if (friend is null) return Array.Empty<IMessengerConsoleMessage>();

        if(_historySentForFriend.TryGetValue(friend.Id, out var sent) && sent) return Array.Empty<IMessengerConsoleMessage>();

        var messageParsed = int.TryParse(messageId, out var messageIdToInt);
        var offsetId = messageParsed ? messageIdToInt : -1;

        using var scope = _scopeFactory.CreateScope();
        var consoleChatLogsRepository = scope.ServiceProvider.GetRequiredService<IConsoleChatLogsRepository>();
        var consoleChatLogs = await consoleChatLogsRepository.GetConsoleChatHistory(Id, friend.Id, offsetId);

        if (consoleChatLogs is null || consoleChatLogs.Count == 0) return Array.Empty<IMessengerConsoleMessage>();

        var now = DateTime.UtcNow;

        var history = new List<IMessengerConsoleMessage>(consoleChatLogs.Count);
        foreach (var log in consoleChatLogs)
        {
            var secondsSinceSent = (int)(now - log.SentAt).TotalSeconds;

            if (log.SenderEntityId == Id)
            {
                history.Add(new MessengerConsoleMessage(friend.Id, secondsSinceSent, log));
            }
            else if (log.RecipientEntityId == Id)
            {
                history.Add(new MessengerConsoleMessage(Id, secondsSinceSent, log, 0));
            }
        }

        _historySentForFriend.TryAdd(friend.Id, true);

        return history;
    }

    public async Task SendMessage(int friendId, string message, int confirmationId)
    {
        if (friendId <= 0 || string.IsNullOrWhiteSpace(message) || friendId == Id) return;

        if (message.Length > 255)
        {
            message = message.Substring(0, 255);
        }

        var friend = GetFriend(friendId);

        if (friend is null) return;

        using var scope = _scopeFactory.CreateScope();
        var consoleChatLogsRepository = scope.ServiceProvider.GetRequiredService<IConsoleChatLogsRepository>();
        var consoleChatLog = await consoleChatLogsRepository.CreateConsoleChatLog(Id, friend.Id, confirmationId, message, DateTime.Now);

        var consoleChatMessage = new MessengerConsoleMessage(friend.Id, 0, consoleChatLog);
        ReceiveMessage(consoleChatMessage);

        var friendPlayer = Player.PlayerManager.GetPlayerById(friend.Id);

        if (friendPlayer is not null)
        {
            consoleChatLog = await consoleChatLogsRepository.SetDelivered(consoleChatLog.Id, DateTime.Now);
            var friendConsoleChatMessage = new MessengerConsoleMessage(Id, 0, consoleChatLog, 0);
            friendPlayer.Messenger.ReceiveMessage(friendConsoleChatMessage);
        }
    }

    private async Task SendPendingMessages()
    {
        using var scope = _scopeFactory.CreateScope();
        var consoleChatLogsRepository = scope.ServiceProvider.GetRequiredService<IConsoleChatLogsRepository>();

        var senders = _pendingConsoleMessages.Values
            .Select(msg => msg.SenderId)
            .Distinct()
            .ToList();

        foreach (var senderId in senders)
        {
            await consoleChatLogsRepository.MarkConversationDelivered(Id, senderId, DateTime.UtcNow);
        }

        foreach (var consoleMessage in _pendingConsoleMessages.Values)
        {
            ReceiveMessage(consoleMessage, true);
        }

        Player.Session.Flush();
    }

    public void ReceiveMessage(IMessengerConsoleMessage consoleMessage, bool queue = false)
    {
        if (consoleMessage is null) return;

        if (!IsInitialized || IsDisposing || IsDisposed)
        {
            return;
        }

        if(!ClientInitialized)
        {
            _pendingConsoleMessages.TryAdd(consoleMessage.Id, consoleMessage);
        }

        var packet = new NewConsoleMessageMessage
        {
            ConsoleMessage = consoleMessage
        };

        if(queue)
        {
            Player.Session.SendQueue(packet);
        } else
        {

            Player.Session.Send(packet);
        }
    }

    public void SendRoomInvite(string message, int[] friendIds) => throw new NotImplementedException();

    protected override async Task OnDispose()
    {
        try
        {
            Friends.Clear();
            Requests.Clear();
            ClientInitialized = false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to dispose Messenger for player {PlayerId}", Player.Id);
        }
    }
}
