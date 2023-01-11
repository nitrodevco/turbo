using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Player;
using Turbo.Furniture.Factories;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;
using Turbo.Core.Game.Players;

namespace Turbo.Rooms.Managers
{
    public class RoomFurnitureManager : IRoomFurnitureManager
    {
        public static readonly int MaxHeight = 40;

        private readonly IRoom _room;
        private readonly IFurnitureFactory _furnitureFactory;
        private readonly IRoomObjectFactory _roomObjectFactory;
        private readonly IPlayerManager _playerManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IDictionary<int, IRoomFloorFurniture> FloorFurniture { get; private set; }
        public IDictionary<int, IRoomWallFurniture> WallFurniture { get; private set; }
        public IDictionary<int, string> FurnitureOwners { get; private set; }

        public IRoomObjectContainer<IRoomObjectFloor> FloorObjects { get; private set; }
        public IRoomObjectContainer<IRoomObjectWall> WallObjects { get; private set; }

        public RoomFurnitureManager(
            IRoom room,
            IFurnitureFactory furnitureFactory,
            IRoomObjectFactory roomObjectFactory,
            IPlayerManager playerManager,
            IServiceScopeFactory serviceScopeFactory)
        {
            _room = room;
            _furnitureFactory = furnitureFactory;
            _roomObjectFactory = roomObjectFactory;
            _playerManager = playerManager;
            _serviceScopeFactory = serviceScopeFactory;

            FloorFurniture = new Dictionary<int, IRoomFloorFurniture>();
            WallFurniture = new Dictionary<int, IRoomWallFurniture>();
            FurnitureOwners = new Dictionary<int, string>();

            FloorObjects = new RoomObjectContainer<IRoomObjectFloor>();
            WallObjects = new RoomObjectContainer<IRoomObjectWall>();
        }

        public async ValueTask InitAsync()
        {
            await LoadFurniture();
        }

        public async ValueTask DisposeAsync()
        {
            // dispose the wall and floor furni
        }

        public IRoomFloorFurniture GetFloorFurniture(int id)
        {
            if (id <= 0) return null;

            if (FloorFurniture.TryGetValue(id, out IRoomFloorFurniture furniture))
            {
                return furniture;
            }

            return null;
        }

        public IRoomWallFurniture GetWallFurniture(int id)
        {
            if (id <= 0) return null;

            if (WallFurniture.TryGetValue(id, out IRoomWallFurniture furniture))
            {
                return furniture;
            }

            return null;
        }

        public IRoomObjectFloor AddFloorRoomObject(IRoomObjectFloor floorObject, IPoint location)
        {
            if (floorObject == null) return null;

            var existingFloorObject = FloorObjects.GetRoomObject(floorObject.Id);

            if (existingFloorObject != null)
            {
                floorObject.Dispose();

                return null;
            }

            floorObject.SetLocation(location);

            if (!floorObject.Logic.OnReady())
            {
                floorObject.Dispose();

                return null;
            }

            _room.RoomMap.AddRoomObjects(floorObject);

            FloorObjects.AddRoomObject(floorObject);

            return floorObject;
        }

        public async Task<IRoomObjectFloor> CreateFloorRoomObjectAndAssign(IRoomObjectFloorHolder floorHolder, IPoint location)
        {
            if (floorHolder == null) return null;

            var floorObject = _roomObjectFactory.CreateFloorObject(_room, FloorObjects, FloorObjects.GetNextId(), floorHolder.LogicType);

            if (floorObject == null) return null;

            if (!floorHolder.SetRoomObject(floorObject) || !await floorHolder.SetupRoomObject())
            {
                floorObject.Dispose();

                return null;
            }

            return AddFloorRoomObject(floorObject, location);
        }

        public IRoomObjectWall AddWallRoomObject(IRoomObjectWall wallObject, string location)
        {
            if (wallObject == null) return null;

            var existingWallObject = WallObjects.GetRoomObject(wallObject.Id);

            if (existingWallObject != null)
            {
                wallObject.Dispose();

                return null;
            }

            wallObject.SetLocation(location);

            if (!wallObject.Logic.OnReady())
            {
                wallObject.Dispose();

                return null;
            }

            _room.RoomMap.AddRoomObjects(wallObject);

            WallObjects.AddRoomObject(wallObject);

            return wallObject;
        }

        public async Task<IRoomObjectWall> CreateWallRoomObjectAndAssign(IRoomObjectWallHolder wallHolder, string location)
        {
            if (wallHolder == null) return null;

            var wallObject = _roomObjectFactory.CreateWallObject(_room, WallObjects, WallObjects.GetNextId(), wallHolder.LogicType);

            if (wallObject == null) return null;

            if (!wallHolder.SetRoomObject(wallObject) || !await wallHolder.SetupRoomObject())
            {
                wallObject.Dispose();

                return null;
            }

            return AddWallRoomObject(wallObject, location);
        }

        public void RemoveFloorFurniture(params int[] objectIds)
        {
            RemoveFloorFurniture(null, objectIds);
        }

        public void RemoveFloorFurniture(IRoomManipulator manipulator, params int[] objectIds)
        {
            List<IRoomObjectFloor> furnitures = new();

            foreach (int objectId in objectIds)
            {
                var furniture = FloorObjects.GetRoomObject(objectId);

                if (furniture != null) furnitures.Add(furniture);
            }

            if (furnitures.Count > 0) RemoveFloorFurniture(manipulator, furnitures.ToArray());
        }

        public void RemoveFloorFurniture(params IRoomObjectFloor[] floorObjects)
        {
            RemoveFloorFurniture(null, floorObjects);
        }

        public void RemoveFloorFurniture(IRoomManipulator manipulator, params IRoomObjectFloor[] floorObjects)
        {
            List<IRoomObjectFloor> removedObjects = new();
            Dictionary<int, List<IRoomFloorFurniture>> playerFurnitures = new();

            foreach (var floorObject in floorObjects)
            {
                if (floorObject.RoomObjectHolder is not IRoomFloorFurniture furniture) continue;

                var pickupType = _room?.RoomSecurityManager?.GetFurniturePickupType(manipulator, furniture) ?? FurniturePickupType.None;

                if (pickupType == FurniturePickupType.None) continue;

                FloorFurniture.Remove(furniture.Id);
                removedObjects.Add(floorObject);

                var pickerId = (pickupType == FurniturePickupType.SendToManipulator) ? manipulator.Id : furniture.PlayerId;

                if (playerFurnitures.TryGetValue(pickerId, out var furnitureList))
                {
                    furnitureList.Add(furniture);
                }
                else
                {
                    furnitureList = new();

                    furnitureList.Add(furniture);

                    playerFurnitures.Add(pickerId, furnitureList);
                }
            }

            if (removedObjects.Count > 0)
            {
                var removedObjectsArray = removedObjects.ToArray();

                FloorObjects.RemoveRoomObject(removedObjectsArray);
                _room.RoomMap.RemoveRoomObjects(null, removedObjectsArray);
            }

            if (playerFurnitures.Count > 0)
            {
                foreach (var pickerId in playerFurnitures.Keys)
                {
                    var furnis = playerFurnitures[pickerId];

                    if (furnis.Count == 0) continue;

                    var player = _playerManager.GetPlayerById(pickerId);

                    foreach (var furniture in furnis)
                    {
                        if (player == null)
                        {
                            furniture.SetRoom(null);
                            furniture.SetPlayer(pickerId);

                            furniture.Dispose();
                        }
                        else
                        {
                            // add to player inventory

                            furniture.Dispose();
                        }
                    }
                }
            }
        }

        public void RemoveWallFurniture(params int[] objectIds)
        {
            RemoveWallFurniture(null, objectIds);
        }

        public void RemoveWallFurniture(IRoomManipulator manipulator, params int[] objectIds)
        {
            List<IRoomObjectWall> furnitures = new();

            foreach (int objectId in objectIds)
            {
                var furniture = WallObjects.GetRoomObject(objectId);

                if (furniture != null) furnitures.Add(furniture);
            }

            if (furnitures.Count > 0) RemoveWallFurniture(manipulator, furnitures.ToArray());
        }

        public void RemoveWallFurniture(params IRoomObjectWall[] wallObjects)
        {
            RemoveWallFurniture(null, wallObjects);
        }

        public void RemoveWallFurniture(IRoomManipulator manipulator, params IRoomObjectWall[] wallObjects)
        {
            List<IRoomObjectWall> removedObjects = new();
            Dictionary<int, List<IRoomWallFurniture>> playerFurnitures = new();

            foreach (var wallObect in wallObjects)
            {
                if (wallObect.RoomObjectHolder is not IRoomWallFurniture furniture) continue;

                var pickupType = _room?.RoomSecurityManager?.GetFurniturePickupType(manipulator, furniture) ?? FurniturePickupType.None;

                if (pickupType == FurniturePickupType.None) continue;

                WallFurniture.Remove(furniture.Id);
                removedObjects.Add(wallObect);

                var pickerId = (pickupType == FurniturePickupType.SendToManipulator) ? manipulator.Id : furniture.PlayerId;

                if (playerFurnitures.TryGetValue(pickerId, out var furnitureList))
                {
                    furnitureList.Add(furniture);
                }
                else
                {
                    furnitureList = new();

                    furnitureList.Add(furniture);

                    playerFurnitures.Add(pickerId, furnitureList);
                }
            }

            if (removedObjects.Count > 0)
            {
                var removedObjectsArray = removedObjects.ToArray();

                WallObjects.RemoveRoomObject(removedObjectsArray);
                _room.RoomMap.RemoveRoomObjects(null, removedObjectsArray);
            }

            if (playerFurnitures.Count > 0)
            {
                foreach (var pickerId in playerFurnitures.Keys)
                {
                    var furnis = playerFurnitures[pickerId];

                    if (furnis.Count == 0) continue;

                    var player = _playerManager.GetPlayerById(pickerId);

                    foreach (var furniture in furnis)
                    {
                        if (player == null)
                        {
                            furniture.SetRoom(null);
                            furniture.SetPlayer(pickerId);

                            furniture.Dispose();
                        }
                        else
                        {
                            // add to player inventory

                            furniture.Dispose();
                        }
                    }
                }
            }
        }

        public bool CanPlaceOnTop(IRoomObjectFloor bottomObject, IRoomObjectFloor topObject)
        {
            if (topObject.Logic is FurnitureStackHelperLogic) return true;

            if (topObject.Logic is FurnitureRollerLogic) return false;

            if (!bottomObject.Logic.CanStack() || bottomObject.Logic.CanSit() || bottomObject.Logic.CanLay()) return false;

            if (bottomObject.Logic is FurnitureRollerLogic)
            {
                if ((topObject.Logic.FurnitureDefinition.X > 1) || (topObject.Logic.FurnitureDefinition.Y > 1)) return false;
            }

            return true;
        }

        public bool IsValidPlacement(IRoomObjectFloor roomObject, IPoint point)
        {
            bool isRotating = roomObject.Location.Rotation != point.Rotation;
            IList<IPoint> affectedPoints = AffectedPoints.GetPoints(roomObject, point);

            if (affectedPoints.Count == 0) return false;

            foreach (IPoint affectedPoint in affectedPoints)
            {
                IRoomTile roomTile = _room.RoomMap.GetTile(affectedPoint);

                if (roomTile == null) return false;

                // do we need to validate that all tiles base height is the same?

                if ((roomTile.Avatars.Count > 0) && !roomObject.Logic.IsOpen()) return false;

                if ((roomTile.Height + roomObject.Logic.StackHeight) > MaxHeight) return false;

                if (roomTile.HasStackHelper) continue;

                if (roomTile.HighestObject != null)
                {
                    if (isRotating && (roomTile.HighestObject == roomObject)) continue;

                    if ((roomTile.HighestObject != roomObject) && !CanPlaceOnTop(roomTile.HighestObject, roomObject)) return false;
                }
            }

            return true;
        }

        public bool MoveFloorFurniture(IRoomManipulator manipulator, int id, int x, int y, Rotation rotation)
        {
            if (id <= 0) return false;

            return MoveFloorFurniture(manipulator, FloorObjects.GetRoomObject(id), new Point(x, y, 0, rotation));
        }

        public bool MoveFloorFurniture(IRoomManipulator manipulator, IRoomObjectFloor roomObject, IPoint location)
        {
            if (roomObject == null || roomObject.RoomObjectHolder is not IRoomFloorFurniture furniture) return false;

            if (!_room?.RoomSecurityManager?.CanManipulateFurniture(manipulator, furniture) ?? false || !IsValidPlacement(roomObject, location))
            {
                if (manipulator != null)
                {
                    // send placement notification

                    manipulator.Session.Send(new ObjectUpdateMessage
                    {
                        Object = roomObject
                    });
                }

                return false;
            }

            IPoint previous = roomObject.Location.Clone();

            int x = location.X;
            int y = location.Y;
            double z = 0;
            Rotation rotation = location.Rotation;

            IRoomTile highestTile = _room.RoomMap.GetHighestTileForRoomObject(roomObject);

            if ((highestTile != null) && (highestTile.HighestObject != roomObject) || highestTile.HasStackHelper) z = highestTile.Height;

            roomObject.SetLocation(new Point(x, y, z, rotation));

            roomObject.Logic.OnMove(manipulator);

            _room.RoomMap.MoveFloorRoomObject(roomObject, previous);

            furniture.Save();

            return true;
        }

        public bool MoveWallFurniture(IRoomManipulator manipulator, int id, string location)
        {
            if (id <= 0) return false;

            return MoveWallFurniture(manipulator, WallObjects.GetRoomObject(id), location);
        }

        public bool MoveWallFurniture(IRoomManipulator manipulator, IRoomObjectWall wallObject, string location)
        {
            if (wallObject == null || wallObject.RoomObjectHolder is not IRoomWallFurniture furniture) return false;

            if (!_room?.RoomSecurityManager?.CanManipulateFurniture(manipulator, furniture) ?? false)
            {
                if (manipulator != null)
                {
                    // send placement notification

                    manipulator.Session.Send(new ItemUpdateMessage
                    {
                        Object = wallObject
                    });
                }

                return false;
            }

            string previous = wallObject.WallLocation;

            wallObject.SetLocation(location);

            wallObject.Logic.OnMove(manipulator);

            _room.RoomMap.MoveWallRoomObject(wallObject, previous);

            furniture.Save();

            return true;
        }

        public IList<IRoomObject> GetRoomObjectsWithLogic(Type type)
        {
            List<IRoomObject> roomObjects = new();

            roomObjects.AddRange(GetFloorRoomObjectsWithLogic(type));
            roomObjects.AddRange(GetWallRoomObjectsWithLogic(type));

            return roomObjects;
        }

        public IList<IRoomObjectFloor> GetFloorRoomObjectsWithLogic(Type type)
        {
            List<IRoomObjectFloor> roomObjects = new();

            foreach (IRoomObjectFloor roomObject in FloorObjects.RoomObjects.Values)
            {
                if (roomObject.Logic.GetType() == type) roomObjects.Add(roomObject);
            }

            return roomObjects;
        }

        public IList<IRoomObjectWall> GetWallRoomObjectsWithLogic(Type type)
        {
            List<IRoomObjectWall> roomObjects = new();

            foreach (IRoomObjectWall roomObject in WallObjects.RoomObjects.Values)
            {
                if (roomObject.Logic.GetType() == type) roomObjects.Add(roomObject);
            }

            return roomObjects;
        }

        public void SendFurnitureToSession(ISession session)
        {
            SendFloorFurnitureToSession(session);
            SendWallFurnitureToSession(session);
        }

        private void SendFloorFurnitureToSession(ISession session)
        {
            List<IRoomObjectFloor> roomObjects = new();
            int count = 0;

            foreach (IRoomObjectFloor roomObject in FloorObjects.RoomObjects.Values)
            {
                roomObjects.Add(roomObject);

                count++;

                if (count == 250)
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

        private void SendWallFurnitureToSession(ISession session)
        {
            List<IRoomObjectWall> roomObjects = new();
            int count = 0;

            foreach (IRoomObjectWall roomObject in WallObjects.RoomObjects.Values)
            {
                roomObjects.Add(roomObject);

                count++;

                if (count == 250)
                {
                    session.Send(new ItemsMessage
                    {
                        Objects = roomObjects,
                        OwnersIdToUsername = FurnitureOwners
                    });

                    roomObjects.Clear();
                    count = 0;
                }
            }

            if (count <= 0) return;

            session.Send(new ItemsMessage
            {
                Objects = roomObjects,
                OwnersIdToUsername = FurnitureOwners
            });
        }

        private async Task LoadFurniture()
        {
            FloorFurniture.Clear();
            WallFurniture.Clear();
            FurnitureOwners.Clear();

            List<FurnitureEntity> entities;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var furnitureRepository = scope.ServiceProvider.GetService<IFurnitureRepository>();
                entities = await furnitureRepository.FindAllByRoomIdAsync(_room.Id);

                List<int> playerIds = new();

                foreach (FurnitureEntity furnitureEntity in entities)
                {
                    if (!playerIds.Contains(furnitureEntity.PlayerEntityId)) playerIds.Add(furnitureEntity.PlayerEntityId);
                }

                if (playerIds.Count > 0)
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

            if (entities == null || entities.Count == 0) return;

            foreach (FurnitureEntity furnitureEntity in entities)
            {
                var definition = _furnitureFactory.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

                if (definition == null) continue;

                if (definition.Type.Equals(FurniType.Floor))
                {
                    var furniture = _furnitureFactory.CreateFloorFurniture(this, furnitureEntity);

                    if (furniture == null) continue;

                    if (!furniture.SetRoom(_room)) continue;

                    FloorFurniture.Add(furniture.Id, furniture);

                    if (FurnitureOwners.TryGetValue(furnitureEntity.PlayerEntityId, out string name))
                    {
                        furniture.PlayerName = name;
                    }

                    await CreateFloorRoomObjectAndAssign(furniture, new Point(furniture.SavedX, furniture.SavedY, furniture.SavedZ, furniture.SavedRotation));

                    continue;
                }

                if (definition.Type.Equals(FurniType.Wall))
                {
                    var furniture = _furnitureFactory.CreateWallFurniture(this, furnitureEntity);

                    if (furniture == null) continue;

                    if (!furniture.SetRoom(_room)) continue;

                    WallFurniture.Add(furniture.Id, furniture);

                    if (FurnitureOwners.TryGetValue(furnitureEntity.PlayerEntityId, out string name))
                    {
                        furniture.PlayerName = name;
                    }

                    await CreateWallRoomObjectAndAssign(furniture, furniture.SavedWallLocation);

                    continue;
                }
            }
        }
    }
}
