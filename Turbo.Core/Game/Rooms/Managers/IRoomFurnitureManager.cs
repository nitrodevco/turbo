using System;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomFurnitureManager : IFurnitureContainer, IRoomObjectContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public void SendFurnitureToSession(ISession session);
    }
}
