using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Database.Factories;
using Turbo.Core.Database.Factories.Rooms;
using Turbo.Core.Game;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Rooms.Factories;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms;

public class RoomManager(
    ILogger<IRoomManager> _logger,
    IRoomFactory _roomFactory,
    IServiceScopeFactory _serviceScopeFactory
) : Component, IRoomManager
{
    private static readonly SemaphoreSlim _roomLock = new(1, 1);
    private readonly IDictionary<int, IRoomModel> _models = new Dictionary<int, IRoomModel>();
    private readonly ConcurrentDictionary<int, IRoom> _rooms = new();

    private int _remainingTryDisposeTicks = DefaultSettings.RoomTryDisposeTicks;

    public async Task<IRoom> GetRoom(int id)
    {
        return await GetOfflineRoom(id);
    }

    public IRoom GetOnlineRoom(int id)
    {
        if (_rooms.TryGetValue(id, out var room))
        {
            room.CancelDispose();

            return room;
        }

        return null;
    }

    public async Task<IRoom> GetOfflineRoom(int id)
    {
        await _roomLock.WaitAsync();

        try
        {
            var room = GetOnlineRoom(id);

            if (room != null) return room;

            using var scope = _serviceScopeFactory.CreateScope();

            var roomRepository = scope.ServiceProvider.GetService<IRoomRepository>();
            var playerRepository = scope.ServiceProvider.GetService<IPlayerRepository>();

            var roomEntity = await roomRepository.FindAsync(id);

            if (roomEntity == null) return null;

            room = _roomFactory.Create(roomEntity);

            room.RoomDetails.PlayerName =
                (await playerRepository.FindUsernameAsync(roomEntity.PlayerEntityId))?.Name ?? "";

            return await AddRoom(room);
        }

        finally
        {
            _roomLock.Release();
        }
    }

    public async Task RemoveRoom(int id)
    {
        var room = GetOnlineRoom(id);

        if (room == null) return;

        if (_rooms.TryRemove(new KeyValuePair<int, IRoom>(room.Id, room))) await room.DisposeAsync();
    }

    public async Task<IRoomModel> GetModel(int id)
    {
        return _models.TryGetValue(id, out var model) ? model : null;
    }

    public IRoomModel GetModelByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || _models.Count == 0) return null;

        foreach (var roomModel in _models.Values)
        {
            if (roomModel == null || !roomModel.Name.Equals(name)) continue;

            return roomModel;
        }

        return null;
    }

    public Task Cycle()
    {
        if (_remainingTryDisposeTicks == 0)
        {
            TryDisposeAllRooms();

            _remainingTryDisposeTicks = DefaultSettings.RoomTryDisposeTicks;
        }

        if (_remainingTryDisposeTicks > -1) _remainingTryDisposeTicks--;

        return Task.WhenAll(_rooms.Values.Select(room => Task.Run(async () => await room.Cycle())));
    }

    protected override async Task OnInit()
    {
        await LoadModels();
    }

    protected override async Task OnDispose()
    {
        await RemoveAllRooms();
    }

    public async Task<IRoom> AddRoom(IRoom room)
    {
        if (room == null) return null;

        var existing = GetOnlineRoom(room.Id);

        if (existing != null)
        {
            if (room != existing) await room.DisposeAsync();

            return existing;
        }

        if (_rooms.TryAdd(room.Id, room)) return room;

        await room.DisposeAsync();

        return null;
    }

    public async Task RemoveAllRooms()
    {
        if (_rooms.Count == 0) return;

        foreach (var id in _rooms.Keys) await RemoveRoom(id);
    }

    public void TryDisposeAllRooms()
    {
        foreach (var room in _rooms.Values) room.TryDispose();
    }

    private async Task LoadModels()
    {
        _models.Clear();

        using var scope = _serviceScopeFactory.CreateScope();
        var roomModelRepository = scope.ServiceProvider.GetService<IRoomModelRepository>();
        var entities = await roomModelRepository.FindAllAsync();

        entities.ForEach(x =>
        {
            IRoomModel roomModel = new RoomModel(x);

            _models.Add(roomModel.Id, roomModel);
        });

        _logger.LogInformation("Loaded {0} room models", _models.Count);
    }
    
    public async Task<List<IRoom>> GetRoomsByOwnerAsync(int ownerId)
    {
        var rooms = new List<IRoom>();

        using var scope = _serviceScopeFactory.CreateScope();
        var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();

        var roomEntities = await roomRepository.FindRoomsByOwnerIdAsync(ownerId);

        foreach (var roomEntity in roomEntities)
        {
            var room = await GetRoom(roomEntity.Id);
            
            if (room != null)
            {
                // Set OwnerName if not already set
                if (string.IsNullOrEmpty(room.RoomDetails.PlayerName))
                {
                    room.RoomDetails.PlayerName = (await playerRepository.FindUsernameAsync(roomEntity.PlayerEntityId))?.Name ?? "Unknown";
                }
                
                if (_rooms.TryGetValue(roomEntity.Id, out var activeRoom))
                {
                    room.RoomDetails.UsersNow = activeRoom.RoomDetails.UsersNow;
                }
                
                rooms.Add(room);
            }
        }

        return rooms;
    }

    public async Task<List<IRoom>> GetRoomsByCategoriesAsync(IEnumerable<int> categoryIds)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();

        var roomEntities = await roomRepository.GetRoomsByCategoryIdsAsync(categoryIds);

        var playerNamesCache = new ConcurrentDictionary<int, string>();

        var roomTasks = roomEntities.Select(async roomEntity =>
        {
            var room = await GetRoom(roomEntity.Id);
            if (room == null)
                return null;

            if (string.IsNullOrEmpty(room.RoomDetails.PlayerName))
            {
                if (!playerNamesCache.TryGetValue(roomEntity.PlayerEntityId, out var playerName))
                {
                    playerName = (await playerRepository.FindUsernameAsync(roomEntity.PlayerEntityId))?.Name ?? "Unknown";
                    playerNamesCache[roomEntity.PlayerEntityId] = playerName;
                }
                room.RoomDetails.PlayerName = playerName;
            }

            return room;
        }).ToList();

        var rooms = await Task.WhenAll(roomTasks);

        return rooms.Where(r => r != null).ToList();
    }
    
    public async Task<List<IRoom>> GetRoomsOrderedByPopularityAsync()
    {
        var rooms = new List<IRoom>();

        using var scope = _serviceScopeFactory.CreateScope();
        var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();

        var roomEntities = await roomRepository.GetRoomsOrderedByPopularityAsync();

        foreach (var roomEntity in roomEntities)
        {
            var room = await GetRoom(roomEntity.Id);
            if (room != null)
            {
                if (string.IsNullOrEmpty(room.RoomDetails.PlayerName))
                {
                    room.RoomDetails.PlayerName = (await playerRepository.FindUsernameAsync(roomEntity.PlayerEntityId))?.Name ?? "Unknown";
                }
                rooms.Add(room);
            }
        }

        return rooms;
    }
    
    public async Task<List<IRoom>> SearchRooms(string searchTerm)
    {
        var rooms = new List<IRoom>();

        using var scope = _serviceScopeFactory.CreateScope();
        var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();

        var roomEntities = await roomRepository.SearchRoomsByNameAsync(searchTerm);

        foreach (var roomEntity in roomEntities)
        {
            var room = await GetRoom(roomEntity.Id);
            if (room != null)
            {
                // Set OwnerName if not already set
                if (string.IsNullOrEmpty(room.RoomDetails.PlayerName))
                {
                    room.RoomDetails.PlayerName = (await playerRepository.FindUsernameAsync(roomEntity.PlayerEntityId))?.Name ?? "Unknown";
                }
                rooms.Add(room);
            }
        }

        return rooms;
    }

}