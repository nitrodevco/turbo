using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Database.Entities.Room;
using Turbo.Database.Repositories.Room;
using Turbo.Rooms.Factories;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public class RoomManager : IRoomManager
    {
        private readonly ILogger<IRoomManager> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRoomFactory _roomFactory;

        private readonly ConcurrentDictionary<int, IRoom> _rooms;
        private readonly IDictionary<int, IRoomModel> _models;

        public RoomManager(
            ILogger<IRoomManager> logger,
            IServiceScopeFactory scopeFactory,
            IRoomObjectFactory roomObjectFactory,
            IRoomModelRepository roomModelRepository,
            IRoomFactory roomFactory)
        {
            _logger = logger;
            _roomFactory = roomFactory;
            _roomObjectFactory = roomObjectFactory;
            _serviceScopeFactory = scopeFactory;

            _rooms = new ConcurrentDictionary<int, IRoom>();
            _models = new Dictionary<int, IRoomModel>();
        }

        public async ValueTask InitAsync()
        {
            await LoadModels();

            // set a interval for TryDisposeAllRooms()
        }

        public async ValueTask DisposeAsync()
        {
            // cancel the TryDisposeAllRooms interval

            await RemoveAllRooms();
        }

        public async Task<IRoom> GetRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room != null) return room;

            return await GetOfflineRoom(id);
        }

        public IRoom GetOnlineRoom(int id)
        {
            if(_rooms.TryGetValue(id, out IRoom room))
            {
                room.CancelDispose();
                return room;
            }

            return null;
        }

        public async Task<IRoom> GetOfflineRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room != null) return room;

            RoomEntity roomEntity;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var roomRepository = scope.ServiceProvider.GetService<IRoomRepository>();
                roomEntity = await roomRepository.FindAsync(id);
            }

            if (roomEntity == null) return null;

            room = _roomFactory.Create(roomEntity);

            return await AddRoom(room);
        }

        public async Task<IRoom> AddRoom(IRoom room)
        {
            if (room == null) return null;

            IRoom existing = GetOnlineRoom(room.Id);

            if (existing != null)
            {
                if (room != existing) await room.DisposeAsync();

                return existing;
            }

            _rooms.TryAdd(room.Id, room);

            return room;
        }

        public async ValueTask RemoveRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room == null) return;

            _rooms.TryRemove(new KeyValuePair<int, IRoom>(room.Id, room));

            await room.DisposeAsync();
        }

        public async ValueTask RemoveAllRooms()
        {
            if (_rooms.Count == 0) return;

            foreach (int id in _rooms.Keys)
            {
                await RemoveRoom(id);
            }
        }

        public void TryDisposeAllRooms()
        {
            foreach (var room in _rooms.Values)
            {
                room.TryDispose();
            }
        }

        public IRoomModel GetModel(int id)
        {
            if(_models.TryGetValue(id, out IRoomModel model))
            {
                return model;
            }

            return null;
        }

        public IRoomModel GetModelByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || _models.Count == 0) return null;

            foreach (IRoomModel roomModel in _models.Values)
            {
                if (roomModel == null || !roomModel.Name.Equals(name)) continue;

                return roomModel;
            }

            return null;
        }

        private async ValueTask LoadModels()
        {
            _models.Clear();

            List<RoomModelEntity> entities;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var roomModelRepository = scope.ServiceProvider.GetService<IRoomModelRepository>();
                entities = await roomModelRepository.FindAllAsync();
            }
                 
            entities.ForEach(x =>
            {
                IRoomModel roomModel = new RoomModel(x);

                _models.Add(roomModel.Id, roomModel);
            });

            _logger.LogInformation("Loaded {0} room models", _models.Count);
        }

        public Task Cycle() => Task.WhenAll(_rooms.Values.Select(x => Task.Run(async () => await x.Cycle())));
    }
}
