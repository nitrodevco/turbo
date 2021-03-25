using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Dtos;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Player;
using Turbo.Furniture.Factories;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Managers
{
    public class RoomFurnitureManager : IRoomFurnitureManager
    {
        private readonly IRoom _room;
        private readonly IFurnitureFactory _furnitureFactory;
        private readonly IRoomObjectFactory _roomObjectFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IDictionary<int, IFurniture> Furniture { get; private set; }
        public IDictionary<int, string> FurnitureOwners { get; private set; }
        public IDictionary<int, IRoomObject> RoomObjects { get; private set; }

        public RoomFurnitureManager(
            IRoom room,
            IFurnitureFactory furnitureFactory,
            IRoomObjectFactory roomObjectFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _room = room;
            _furnitureFactory = furnitureFactory;
            _roomObjectFactory = roomObjectFactory;
            _serviceScopeFactory = serviceScopeFactory;

            Furniture = new Dictionary<int, IFurniture>();
            FurnitureOwners = new Dictionary<int, string>();
            RoomObjects = new Dictionary<int, IRoomObject>();
        }

        public async ValueTask InitAsync()
        {
            await LoadFurniture();
        }

        public async ValueTask DisposeAsync()
        {
            RemoveAllFurniture();
        }

        public IFurniture GetFurniture(int id)
        {
            if (id <= 0) return null;

            if (Furniture.TryGetValue(id, out IFurniture furniture))
            {
                return furniture;
            }

            return null;
        }

        public IRoomObject GetRoomObject(int id)
        {
            if (id < 0) return null;

            if (RoomObjects.TryGetValue(id, out IRoomObject roomObject))
            {
                return roomObject;
            }

            return null;
        }

        public IRoomObject AddRoomObject(IRoomObject roomObject, IPoint location)
        {
            if (roomObject == null) return null;

            IRoomObject existingRoomObject = GetRoomObject(roomObject.Id);

            if (existingRoomObject != null)
            {
                roomObject.Dispose();

                return null;
            }

            if (roomObject.Logic is not IFurnitureLogic furnitureLogic) return null;

            roomObject.SetLocation(location);

            _room.RoomMap.AddRoomObjects(roomObject);

            RoomObjects.Add(roomObject.Id, roomObject);

            return roomObject;
        }

        public IRoomObject CreateRoomObjectAndAssign(IRoomObjectFurnitureHolder furnitureHolder, IPoint location)
        {
            if (furnitureHolder == null) return null;

            IRoomObject roomObject = _roomObjectFactory.Create(_room, this, furnitureHolder.Id, furnitureHolder.Type, furnitureHolder.LogicType);

            if (roomObject == null) return null;

            if (!furnitureHolder.SetRoomObject(roomObject)) return null;

            return AddRoomObject(roomObject, location);
        }

        public void RemoveFurniture(int id)
        {
            IFurniture furniture = GetFurniture(id);

            if (furniture == null) return;

            Furniture.Remove(id);

            furniture.Dispose();
        }

        public void RemoveAllFurniture()
        {
            foreach (int id in Furniture.Keys) RemoveFurniture(id);
        }

        public void RemoveRoomObject(int id)
        {
            IRoomObject roomObject = GetRoomObject(id);

            if (roomObject == null) return;

            _room.RoomMap.RemoveRoomObjects(null, roomObject);

            RoomObjects.Remove(id);

            roomObject.Dispose();
        }

        public void RemoveAllRoomObjects()
        {
            foreach (int id in RoomObjects.Keys) RemoveRoomObject(id);
        }

        public void SendFurnitureToSession(ISession session)
        {
            List<IRoomObject> roomObjects = new();
            int count = 0;

            foreach(IRoomObject roomObject in RoomObjects.Values)
            {
                roomObjects.Add(roomObject);

                count++;

                if(count == 250)
                {
                    session.Send(new ObjectsMessage
                    {
                        Objects = roomObjects,
                        OwnersIdToUsername = FurnitureOwners
                    });

                    roomObjects.Clear();
                    count = 0;
                }
            }

            if (count <= 0) return;

            session.Send(new ObjectsMessage
            {
                Objects = roomObjects,
                OwnersIdToUsername = FurnitureOwners
            });
        }

        private async ValueTask LoadFurniture()
        {
            Furniture.Clear();
            FurnitureOwners.Clear();

            List<FurnitureEntity> entities;
            List<int> playerIds = new();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var furnitureRepository = scope.ServiceProvider.GetService<IFurnitureRepository>();
                entities = await furnitureRepository.FindAllByRoomIdAsync(_room.Id);

                foreach (FurnitureEntity furnitureEntity in entities)
                {
                    playerIds.Add(furnitureEntity.PlayerEntityId);
                }

                if(playerIds.Count > 0)
                {
                    var playerRepository = scope.ServiceProvider.GetService<IPlayerRepository>();

                    IList<PlayerUsernameDto> usernames = await playerRepository.FindUsernamesAsync(playerIds);

                    if (usernames.Count > 0)
                    {
                        foreach (PlayerUsernameDto dto in usernames)
                        {
                            FurnitureOwners.Add(dto.Id, dto.Name);
                        }
                    }
                }
            }

            foreach (FurnitureEntity furnitureEntity in entities)
            {
                IFurniture furniture = _furnitureFactory.Create(this, furnitureEntity);

                if (!furniture.SetRoom(_room)) continue;

                if(FurnitureOwners.TryGetValue(furnitureEntity.PlayerEntityId, out string name))
                {
                    furniture.PlayerName = name;
                }

                Furniture.Add(furniture.Id, furniture);

                CreateRoomObjectAndAssign(furniture, new Point(furnitureEntity.X, furnitureEntity.Y, furnitureEntity.Z, furnitureEntity.Rotation));
            }
        }
    }
}
