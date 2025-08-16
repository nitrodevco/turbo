using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Messenger;
using Turbo.Database.Repositories.Messenger;
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

    public ConcurrentDictionary<int, IMessengerCategory> Categories => _categories;
    public ConcurrentDictionary<int, IMessengerFriend> Friends => _friends;
    public ConcurrentDictionary<int, IMessengerRequest> Requests => _requests;

    protected override async Task OnInit()
    {
        try
        {
            await LoadFriends();
            await LoadRequests();
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

    private async Task LoadCategories() => throw new NotImplementedException("Loading categories is not implemented yet.");

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
        Friends.TryAdd(messengerFriend.Id, messengerFriend);
        _updates.AddAdded(messengerFriend);
        CommitUpdates();
    }

    public async Task AcceptFriends(int[] playerIds)
    {
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
        Friends.TryRemove(friendId, out _);
        _updates.AddRemoved(friendId);
        CommitUpdates();
    }

    public async Task RemoveFriends(int[] friendIds)
    {
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
        friend.InRoom = player.RoomObject is not null;

        _updates.AddUpdated(friend);

        if (commit)
        {
            CommitUpdates();
        }
    }

    public async Task SendUpdateToFriends(bool commit = false)
    {
        foreach (var friend in Friends.Values)
        {
            if (friend is null || friend.Status is PlayerStatusEnum.Offline) continue;

            var player = await Player.PlayerManager.GetOfflinePlayerById(friend.Id);

            if (player is not null && player.IsInitialized)
            {
                player.Messenger.UpdateFriend(Player);
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
        if (!IsInitialized || IsDisposing || IsDisposed || !_updates.HasUpdates) return;

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

    public void SendMessage(int friendId, string message) => throw new NotImplementedException();

    public void SendRoomInvite(string message, int[] friendIds) => throw new NotImplementedException();

    protected override async Task OnDispose()
    {
        try
        {
            Friends.Clear();
            Requests.Clear();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to dispose Messenger for player {PlayerId}", Player.Id);
        }
    }
}
