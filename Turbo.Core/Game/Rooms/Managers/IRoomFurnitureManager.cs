using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomFurnitureManager : IComponent
{
    public IDictionary<int, string> FurnitureOwners { get; }
    public IRoomObjectContainer<IRoomObjectFloor> FloorObjects { get; }
    public IRoomObjectContainer<IRoomObjectWall> WallObjects { get; }

    public IRoomFloorFurniture GetFloorFurniture(int id);
    public IRoomWallFurniture GetWallFurniture(int id);
    public IRoomObjectFloor AddFloorRoomObject(IRoomObjectFloor roomObject, IPoint location);

    public Task<IRoomObjectFloor> CreateFloorRoomObjectAndAssign(IRoomObjectFloorHolder furnitureHolder,
        IPoint location);

    public void RemoveFloorRoomObject(IRoomObjectFloor floorObject);
    public void RemoveFloorFurnitureById(IRoomManipulator manipulator, int furniId);
    public void RemoveFloorFurnitureByObjectId(IRoomManipulator manipulator, int objectId);
    public void RemoveFloorFurniture(IRoomManipulator manipulator, IRoomFloorFurniture floorFurniture);
    public IRoomObjectWall AddWallRoomObject(IRoomObjectWall wallObject, string location);
    public Task<IRoomObjectWall> CreateWallRoomObjectAndAssign(IRoomObjectWallHolder wallHolder, string location);
    public void RemoveWallRoomObject(IRoomObjectWall wallObject);
    public void RemoveWallFurnitureById(IRoomManipulator manipulator, int furniId);
    public void RemoveWallFurnitureByObjectId(IRoomManipulator manipulator, int objectId);
    public void RemoveWallFurniture(IRoomManipulator manipulator, IRoomWallFurniture wallFurniture);

    public bool CanPlaceOnTop(IRoomObjectFloor bottomObject, IRoomObjectFloor topLogic);
    public bool IsValidPlacement(IRoomObjectFloor roomObject, IPoint point);
    public bool MoveFloorFurniture(IRoomManipulator manipulator, int id, int x, int y, Rotation rotation);
    public bool MoveFloorFurniture(IRoomManipulator manipulator, IRoomObjectFloor roomObject, IPoint location);
    public Task<bool> PlaceFloorFurnitureByFurniId(IPlayer player, int furniId, IPoint location);
    public bool MoveWallFurniture(IRoomManipulator manipulator, int id, string location);
    public bool MoveWallFurniture(IRoomManipulator manipulator, IRoomObjectWall roomObject, string location);
    public Task<bool> PlaceWallFurnitureByFurniId(IPlayer player, int furniId, string location);
    public IList<IRoomObject> GetRoomObjectsWithLogic(Type type);
    public IList<IRoomObjectFloor> GetFloorRoomObjectsWithLogic(Type type);
    public IList<IRoomObjectWall> GetWallRoomObjectsWithLogic(Type type);
    public void SendFurnitureToSession(ISession session);
}