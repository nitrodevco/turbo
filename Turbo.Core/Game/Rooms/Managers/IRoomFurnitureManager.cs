using System;
using System.Collections.Generic;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomFurnitureManager : IFurnitureContainer, IRoomObjectContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public bool CanPlaceOnTop(IRoomObject bottom, IRoomObject top);
        public bool IsValidPlacement(IRoomObject roomObject, IPoint point);
        public bool MoveFurniture(IRoomManipulator manipulator, int id, int x, int y, Rotation rotation, bool skipChecks = false);
        public IList<IRoomObject> GetRoomObjectsWithLogic(Type type);
        public void SendFurnitureToSession(ISession session);
    }
}
