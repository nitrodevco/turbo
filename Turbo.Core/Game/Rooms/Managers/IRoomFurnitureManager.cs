using System;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomFurnitureManager : IFurnitureContainer, IRoomObjectContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public bool MoveFurniture(IRoomManipulator manipulator, int id, int x, int y, Rotation rotation, bool skipChecks = false);
        public void SendFurnitureToSession(ISession session);
    }
}
