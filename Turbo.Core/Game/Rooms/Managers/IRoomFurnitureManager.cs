using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomFurnitureManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IRoomObjectContainer<IRoomObjectFloor> FloorObjects { get; }
        public IRoomObjectContainer<IRoomObjectWall> WallObjects { get; }

        public IRoomFloorFurniture GetFloorFurniture(int id);
        public IRoomWallFurniture GetWallFurniture(int id);
        public IRoomObjectFloor AddFloorRoomObject(IRoomObjectFloor roomObject, IPoint location);
        public Task<IRoomObjectFloor> CreateFloorRoomObjectAndAssign(IRoomObjectFloorHolder furnitureHolder, IPoint location);
        public IRoomObjectWall AddWallRoomObject(IRoomObjectWall roomObject, string location);
        public Task<IRoomObjectWall> CreateWallRoomObjectAndAssign(IRoomObjectWallHolder furnitureHolder, string location);
        public void RemoveFloorFurniture(params int[] objectIds);
        public void RemoveFloorFurniture(IRoomManipulator manipulator, params int[] objectIds);
        public void RemoveFloorFurniture(params IRoomObjectFloor[] floorObjects);
        public void RemoveFloorFurniture(IRoomManipulator manipulator, params IRoomObjectFloor[] floorObjects);
        public void RemoveWallFurniture(params int[] objectIds);
        public void RemoveWallFurniture(IRoomManipulator manipulator, params int[] objectIds);
        public void RemoveWallFurniture(params IRoomObjectWall[] wallObjects);
        public void RemoveWallFurniture(IRoomManipulator manipulator, params IRoomObjectWall[] wallObjects);
        public bool CanPlaceOnTop(IRoomObjectFloor bottomObject, IRoomObjectFloor topLogic);
        public bool IsValidPlacement(IRoomObjectFloor roomObject, IPoint point);
        public bool MoveFloorFurniture(IRoomManipulator manipulator, int id, int x, int y, Rotation rotation);
        public bool MoveFloorFurniture(IRoomManipulator manipulator, IRoomObjectFloor roomObject, IPoint location);
        public bool MoveWallFurniture(IRoomManipulator manipulator, int id, string location);
        public bool MoveWallFurniture(IRoomManipulator manipulator, IRoomObjectWall roomObject, string location);
        public IList<IRoomObject> GetRoomObjectsWithLogic(Type type);
        public IList<IRoomObjectFloor> GetFloorRoomObjectsWithLogic(Type type);
        public IList<IRoomObjectWall> GetWallRoomObjectsWithLogic(Type type);
        public void SendFurnitureToSession(ISession session);
    }
}
