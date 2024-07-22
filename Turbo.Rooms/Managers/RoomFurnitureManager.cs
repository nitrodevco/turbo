using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Events;
using Turbo.Core.Game;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Player;
using Turbo.Events.Game.Rooms.Furniture;
using Turbo.Furniture.Factories;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Managers;

public class RoomFurnitureManager : Component, IRoomFurnitureManager
{
    private readonly ITurboEventHub _eventHub;
    private readonly IFurnitureFactory _furnitureFactory;
    private readonly IPlayerManager _playerManager;
    private readonly IRoom _room;
    private readonly IRoomObjectFactory _roomObjectFactory;
    private readonly IRoomObjectLogicFactory _roomObjectLogicFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly IDictionary<int, int> _pendingPickerIds = new Dictionary<int, int>();

    public RoomFurnitureManager(
        IRoom room,
        ITurboEventHub turboEventHub,
        IFurnitureFactory furnitureFactory,
        IRoomObjectFactory roomObjectFactory,
        IRoomObjectLogicFactory roomObjectLogicFactory,
        IPlayerManager playerManager,
        IServiceScopeFactory serviceScopeFactory)
    {
        _room = room;
        _eventHub = turboEventHub;
        _furnitureFactory = furnitureFactory;
        _roomObjectFactory = roomObjectFactory;
        _roomObjectLogicFactory = roomObjectLogicFactory;
        _playerManager = playerManager;
        _serviceScopeFactory = serviceScopeFactory;

        FloorObjects = new RoomObjectContainer<IRoomObjectFloor>(RemoveFloorRoomObject);
        WallObjects = new RoomObjectContainer<IRoomObjectWall>(RemoveWallRoomObject);
    }

    public IDictionary<int, IRoomFloorFurniture> FloorFurniture { get; } = new Dictionary<int, IRoomFloorFurniture>();
    public IDictionary<int, IRoomWallFurniture> WallFurniture { get; } = new Dictionary<int, IRoomWallFurniture>();
    public IDictionary<int, string> FurnitureOwners { get; } = new Dictionary<int, string>();

    public IRoomObjectContainer<IRoomObjectFloor> FloorObjects { get; }
    public IRoomObjectContainer<IRoomObjectWall> WallObjects { get; }

    public IRoomFloorFurniture GetFloorFurniture(int id)
    {
        if (id <= 0) return null;

        if (FloorFurniture.TryGetValue(id, out var furniture)) return furniture;

        return null;
    }

    public IRoomWallFurniture GetWallFurniture(int id)
    {
        if (id <= 0) return null;

        if (WallFurniture.TryGetValue(id, out var furniture)) return furniture;

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

        floorObject.SetLocation(location, false, false);

        if (!floorObject.Logic.OnReady())
        {
            floorObject.Dispose();

            return null;
        }

        _room.RoomMap.AddFloorObject(floorObject);

        FloorObjects.AddRoomObject(floorObject);

        return floorObject;
    }

    public async Task<IRoomObjectFloor> CreateFloorRoomObjectAndAssign(IRoomObjectFloorHolder floorHolder,
        IPoint location)
    {
        if (floorHolder == null) return null;

        var floorObject =
            _roomObjectFactory.CreateFloorObject(_room, FloorObjects, floorHolder.Id, floorHolder.LogicType);

        if (floorObject == null) return null;

        if (!floorHolder.SetRoomObject(floorObject) || !await floorHolder.SetupRoomObject())
        {
            floorObject.Dispose();

            return null;
        }

        return AddFloorRoomObject(floorObject, location);
    }

    public void RemoveFloorRoomObject(IRoomObjectFloor floorObject)
    {
        if (floorObject == null || floorObject.Disposed) return;

        FloorObjects.RemoveRoomObject(floorObject);

        var pickerId = -1;

        if (_pendingPickerIds.Remove(floorObject.Id, out var picker)) pickerId = picker;

        _room.RoomMap.RemoveFloorObject(floorObject, pickerId);

        floorObject.Dispose();
    }

    public void RemoveFloorFurnitureById(IRoomManipulator manipulator, int furniId)
    {
        if (furniId < 0) return;

        var floorFurniture = GetFloorFurniture(furniId);

        if (floorFurniture == null) return;

        RemoveFloorFurniture(manipulator, floorFurniture);
    }

    public void RemoveFloorFurnitureByObjectId(IRoomManipulator manipulator, int objectId)
    {
        if (objectId < 0) return;

        var floorObject = FloorObjects.GetRoomObject(objectId);

        if (floorObject == null) return;

        if (floorObject.RoomObjectHolder is not IRoomFloorFurniture floorFurniture) return;

        RemoveFloorFurniture(manipulator, floorFurniture);
    }

    public void RemoveFloorFurniture(IRoomManipulator manipulator, IRoomFloorFurniture floorFurniture)
    {
        var message = _eventHub.Dispatch(new RemoveFloorFurnitureEvent
        {
            Manipulator = manipulator,
            Furniture = floorFurniture
        });

        if (message.IsCancelled) return;

        if (floorFurniture == null) return;

        var pickupType = _room?.RoomSecurityManager?.GetFurniturePickupType(manipulator, floorFurniture) ??
                         FurniturePickupType.None;

        if (pickupType == FurniturePickupType.None) return;

        var pickerId = pickupType == FurniturePickupType.SendToManipulator ? manipulator.Id : floorFurniture.PlayerId;

        _pendingPickerIds.Add(floorFurniture.Id, pickerId);

        FloorFurniture.Remove(floorFurniture.Id);
        floorFurniture.ClearRoomObject();

        var player = _playerManager.GetPlayerById(pickerId);

        if (player == null)
        {
            floorFurniture.SetRoom(null);
            floorFurniture.SetPlayer(pickerId);

            floorFurniture.Dispose();

            return;
        }

        player.PlayerInventory?.FurnitureInventory?.AddFurnitureFromRoom(floorFurniture);

        floorFurniture.Dispose();
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

        _room.RoomMap.AddWallObject(wallObject);

        WallObjects.AddRoomObject(wallObject);

        return wallObject;
    }

    public async Task<IRoomObjectWall> CreateWallRoomObjectAndAssign(IRoomObjectWallHolder wallHolder, string location)
    {
        if (wallHolder == null) return null;

        var wallObject = _roomObjectFactory.CreateWallObject(_room, WallObjects, wallHolder.Id, wallHolder.LogicType);

        if (wallObject == null) return null;

        if (!wallHolder.SetRoomObject(wallObject) || !await wallHolder.SetupRoomObject())
        {
            wallObject.Dispose();

            return null;
        }

        return AddWallRoomObject(wallObject, location);
    }

    public void RemoveWallRoomObject(IRoomObjectWall wallObject)
    {
        if (wallObject == null || wallObject.Disposed) return;

        WallObjects.RemoveRoomObject(wallObject);

        var pickerId = -1;

        if (_pendingPickerIds.Remove(wallObject.Id, out var picker)) pickerId = picker;

        _room.RoomMap.RemoveWallObject(wallObject, pickerId);

        wallObject.Dispose();
    }

    public void RemoveWallFurnitureById(IRoomManipulator manipulator, int furniId)
    {
        if (furniId < 0) return;

        var wallFurniture = GetWallFurniture(furniId);

        if (wallFurniture == null) return;

        RemoveWallFurniture(manipulator, wallFurniture);
    }

    public void RemoveWallFurnitureByObjectId(IRoomManipulator manipulator, int objectId)
    {
        if (objectId < 0) return;

        var wallObject = WallObjects.GetRoomObject(objectId);

        if (wallObject == null) return;

        if (wallObject.RoomObjectHolder is not IRoomWallFurniture wallFurniture) return;

        RemoveWallFurniture(manipulator, wallFurniture);
    }

    public void RemoveWallFurniture(IRoomManipulator manipulator, IRoomWallFurniture wallFurniture)
    {
        var message = _eventHub.Dispatch(new RemoveWallFurnitureEvent
        {
            Manipulator = manipulator,
            Furniture = wallFurniture
        });

        if (message.IsCancelled) return;

        if (wallFurniture == null) return;

        var pickupType = _room?.RoomSecurityManager?.GetFurniturePickupType(manipulator, wallFurniture) ??
                         FurniturePickupType.None;

        if (pickupType == FurniturePickupType.None) return;

        var pickerId = pickupType == FurniturePickupType.SendToManipulator ? manipulator.Id : wallFurniture.PlayerId;

        _pendingPickerIds.Add(wallFurniture.Id, pickerId);

        WallFurniture.Remove(wallFurniture.Id);
        wallFurniture.ClearRoomObject();

        var player = _playerManager.GetPlayerById(pickerId);

        if (player == null)
        {
            wallFurniture.SetRoom(null);
            wallFurniture.SetPlayer(pickerId);

            wallFurniture.Dispose();

            return;
        }

        player.PlayerInventory?.FurnitureInventory?.AddFurnitureFromRoom(wallFurniture);

        wallFurniture.Dispose();
    }

    public bool CanPlaceOnTop(IRoomObjectFloor bottomObject, IRoomObjectFloor topObject)
    {
        if (topObject.Logic is FurnitureStackHelperLogic) return true;

        if (topObject.Logic is FurnitureRollerLogic) return false;

        if (!bottomObject.Logic.CanStack() || bottomObject.Logic.CanSit() || bottomObject.Logic.CanLay()) return false;

        if (bottomObject.Logic is FurnitureRollerLogic)
            if (topObject.Logic.FurnitureDefinition.X > 1 || topObject.Logic.FurnitureDefinition.Y > 1)
                return false;

        return true;
    }

    public bool IsValidPlacement(IRoomObjectFloor roomObject, IPoint point)
    {
        var isRotating = roomObject.Location.Compare(point) && roomObject.Location.Rotation != point.Rotation;
        var affectedPoints = AffectedPoints.GetPoints(roomObject, point);

        if (affectedPoints == null || affectedPoints.Count == 0) return false;

        foreach (var affectedPoint in affectedPoints)
        {
            var roomTile = _room.RoomMap.GetTile(affectedPoint);

            if (roomTile == null) return false;

            // do we need to validate that all tiles base height is the same?

            if (roomTile.Avatars.Count > 0 && !isRotating) return false;

            if (roomTile.Height + roomObject.Logic.StackHeight > DefaultSettings.MaximumFurnitureHeight) return false;

            if (roomTile.HasStackHelper) continue;

            if (roomTile.HighestObject != null)
            {
                if (isRotating && roomTile.HighestObject == roomObject) continue;

                if (roomTile.HighestObject != roomObject && !CanPlaceOnTop(roomTile.HighestObject, roomObject))
                    return false;
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
        var message = _eventHub.Dispatch(new MoveFloorFurnitureEvent
        {
            Manipulator = manipulator,
            FloorObject = roomObject
        });

        if (message.IsCancelled) return false;

        if (roomObject == null || roomObject.RoomObjectHolder is not IRoomFloorFurniture furniture) return false;

        if (!_room.RoomSecurityManager.CanManipulateFurniture(manipulator, furniture) ||
            !IsValidPlacement(roomObject, location))
        {
            if (manipulator != null)
                // send placement notification
                manipulator.Session.Send(new ObjectUpdateMessage
                {
                    Object = roomObject
                });

            return false;
        }

        var previous = roomObject.Location.Clone();

        roomObject.X = location.X;
        roomObject.Y = location.Y;
        roomObject.Rotation = location.Rotation;

        var tile = _room.RoomMap.GetTile(roomObject.Location);

        if ((tile != null && tile.HighestObject != roomObject) || tile.HasStackHelper) roomObject.Z = tile.Height;

        _room.RoomMap.MoveFloorRoomObject(roomObject, previous);

        roomObject.Logic.OnMove(manipulator);

        furniture.Save();

        return true;
    }

    public async Task<bool> PlaceFloorFurnitureByFurniId(IPlayer player, int furniId, IPoint location)
    {
        var message = await _eventHub.DispatchAsync(new PlaceFloorFurnitureEvent
        {
            Player = player,
            FurniId = furniId,
            Location = location
        });

        if (message.IsCancelled) return false;

        if (player == null || furniId < 0 || location == null) return false;

        var playerFurniture = player.PlayerInventory?.FurnitureInventory?.GetFurniture(furniId);

        if (playerFurniture == null) return false;

        if (!_room.RoomSecurityManager.CanPlaceFurniture(player) ||
            !IsValidPlacement(playerFurniture.FurnitureDefinition, location))
            // cant place here
            return false;

        var newLocation = new Point(location.X, location.Y, 0, location.Rotation);

        var tile = _room.RoomMap.GetTile(newLocation);

        if (tile == null) return false;

        newLocation.Z = tile.Height;

        var furniture = _furnitureFactory.CreateFloorFurnitureFromPlayerFurniture(this, playerFurniture);

        if (furniture == null) return false;

        furniture.SetRoom(_room);

        if (!furniture.SetPlayer(player)) return false;

        playerFurniture.Dispose();

        FloorFurniture.Add(furniture.Id, furniture);

        if (!FurnitureOwners.ContainsKey(player.Id)) FurnitureOwners.Add(player.Id, player.Name);

        var floorObject = await CreateFloorRoomObjectAndAssign(furniture, newLocation);

        if (floorObject != null) floorObject.Logic.OnPlace(player);

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
        var message = _eventHub.Dispatch(new MoveWallFurnitureEvent
        {
            Manipulator = manipulator,
            WallObject = wallObject,
            Location = location
        });

        if (message.IsCancelled) return false;

        if (wallObject == null || wallObject.RoomObjectHolder is not IRoomWallFurniture furniture) return false;

        if (!_room.RoomSecurityManager.CanManipulateFurniture(manipulator, furniture))
        {
            if (manipulator != null)
                // send placement notification
                manipulator.Session.Send(new ItemUpdateMessage
                {
                    Object = wallObject
                });

            return false;
        }

        var previous = wallObject.WallLocation;

        wallObject.SetLocation(location);

        _room.RoomMap.MoveWallRoomObject(wallObject, previous);

        wallObject.Logic.OnMove(manipulator);

        furniture.Save();

        return true;
    }

    public async Task<bool> PlaceWallFurnitureByFurniId(IPlayer player, int furniId, string location)
    {
        var message = await _eventHub.DispatchAsync(new PlaceWallFurnitureEvent
        {
            Player = player,
            FurniId = furniId,
            Location = location
        });

        if (message.IsCancelled) return false;

        if (player == null || furniId < 0 || location.Length == 0) return false;

        var playerFurniture = player.PlayerInventory?.FurnitureInventory?.GetFurniture(furniId);

        if (playerFurniture == null) return false;

        if (!_room.RoomSecurityManager.CanPlaceFurniture(player))
            // cant place here
            return false;

        var furniture = _furnitureFactory.CreateWallFurnitureFromPlayerFurniture(this, playerFurniture);

        if (furniture == null) return false;

        furniture.SetRoom(_room);

        if (!furniture.SetPlayer(player)) return false;

        playerFurniture.Dispose();

        WallFurniture.Add(furniture.Id, furniture);

        FurnitureOwners.TryAdd(player.Id, player.Name);

        var wallObject = await CreateWallRoomObjectAndAssign(furniture, location);

        if (wallObject != null) wallObject.Logic.OnPlace(player);

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

        foreach (var roomObject in FloorObjects.RoomObjects.Values)
            if (roomObject.Logic.GetType() == type)
                roomObjects.Add(roomObject);

        return roomObjects;
    }

    public IList<IRoomObjectWall> GetWallRoomObjectsWithLogic(Type type)
    {
        List<IRoomObjectWall> roomObjects = new();

        foreach (var roomObject in WallObjects.RoomObjects.Values)
            if (roomObject.Logic.GetType() == type)
                roomObjects.Add(roomObject);

        return roomObjects;
    }

    public void SendFurnitureToSession(ISession session)
    {
        SendFloorFurnitureToSession(session);
        SendWallFurnitureToSession(session);
    }

    protected override async Task OnInit()
    {
        await LoadFurniture();
    }

    protected override async Task OnDispose()
    {
    }

    public bool CanPlaceOnTop(IRoomObjectFloor bottomObject, IFurnitureDefinition furnitureDefinition)
    {
        var logicType = _roomObjectLogicFactory.GetLogicType(furnitureDefinition.Logic);

        if (logicType != null)
        {
            if (logicType.IsAssignableFrom(typeof(FurnitureStackHelperLogic))) return true;

            if (logicType.IsAssignableFrom(typeof(FurnitureRollerLogic))) return false;
        }

        if (!bottomObject.Logic.CanStack() || bottomObject.Logic.CanSit() || bottomObject.Logic.CanLay()) return false;

        if (bottomObject.Logic is FurnitureRollerLogic)
            if (furnitureDefinition.X > 1 || furnitureDefinition.Y > 1)
                return false;

        return true;
    }

    public bool IsValidPlacement(IFurnitureDefinition furnitureDefinition, IPoint point)
    {
        if (furnitureDefinition == null || point == null) return false;

        var affectedPoints = AffectedPoints.GetPoints(furnitureDefinition.X, furnitureDefinition.Y, point);

        if (affectedPoints == null || affectedPoints.Count == 0) return false;

        foreach (var affectedPoint in affectedPoints)
        {
            var roomTile = _room.RoomMap.GetTile(affectedPoint);

            if (roomTile == null) return false;

            // do we need to validate that all tiles base height is the same?

            if (roomTile.Avatars.Count > 0) return false;

            if (roomTile.Height + furnitureDefinition.Z > DefaultSettings.MaximumFurnitureHeight) return false;

            if (roomTile.HasStackHelper) continue;

            if (roomTile.HighestObject != null)
                if (!CanPlaceOnTop(roomTile.HighestObject, furnitureDefinition))
                    return false;
        }

        return true;
    }

    private void SendFloorFurnitureToSession(ISession session)
    {
        List<IRoomObjectFloor> roomObjects = new();
        var count = 0;

        foreach (var roomObject in FloorObjects.RoomObjects.Values)
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
        var count = 0;

        foreach (var roomObject in WallObjects.RoomObjects.Values)
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

        using var scope = _serviceScopeFactory.CreateScope();

        var furnitureRepository = scope.ServiceProvider.GetService<IFurnitureRepository>();
        var furnitureEntities = await furnitureRepository.FindAllByRoomIdAsync(_room.Id);

        if (furnitureEntities == null || furnitureEntities.Count == 0) return;

        List<int> playerIds = [];

        foreach (var furnitureEntity in furnitureEntities)
            if (!playerIds.Contains(furnitureEntity.PlayerEntityId))
                playerIds.Add(furnitureEntity.PlayerEntityId);

        if (playerIds.Count > 0)
        {
            var _playerRepository = scope.ServiceProvider.GetService<IPlayerRepository>();
            var usernames = await _playerRepository.FindUsernamesAsync(playerIds);

            if (usernames.Count > 0)
                foreach (var dto in usernames)
                    if (!FurnitureOwners.ContainsKey(dto.Id))
                        FurnitureOwners.Add(dto.Id, dto.Name);
        }

        foreach (var furnitureEntity in furnitureEntities)
        {
            var definition = _furnitureFactory.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (definition == null) continue;

            if (definition.Type.Equals(FurniType.Floor))
            {
                if (FloorFurniture.ContainsKey(furnitureEntity.Id)) continue;

                var furniture = _furnitureFactory.CreateFloorFurniture(this, furnitureEntity);

                if (furniture == null) continue;

                furniture.SetRoom(_room);

                if (FurnitureOwners.TryGetValue(furnitureEntity.PlayerEntityId, out var name))
                    furniture.SetPlayer(furnitureEntity.PlayerEntityId, name);

                FloorFurniture.Add(furniture.Id, furniture);

                await CreateFloorRoomObjectAndAssign(furniture,
                    new Point(furniture.SavedX, furniture.SavedY, furniture.SavedZ, furniture.SavedRotation));

                continue;
            }

            if (definition.Type.Equals(FurniType.Wall))
            {
                if (WallFurniture.ContainsKey(furnitureEntity.Id)) continue;

                var furniture = _furnitureFactory.CreateWallFurniture(this, furnitureEntity);

                if (furniture == null) continue;

                furniture.SetRoom(_room);

                if (FurnitureOwners.TryGetValue(furnitureEntity.PlayerEntityId, out var name))
                    furniture.SetPlayer(furnitureEntity.PlayerEntityId, name);

                WallFurniture.Add(furniture.Id, furniture);

                await CreateWallRoomObjectAndAssign(furniture, furniture.SavedWallLocation);
            }
        }
    }
}