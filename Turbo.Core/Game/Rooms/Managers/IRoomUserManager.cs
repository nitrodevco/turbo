using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomUserManager : IRoomObjectContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public IRoomObject GetRoomObjectByUserId(int userId);
        public IRoomObject GetRoomObjectByUsername(string username);
        public void EnterRoom(IRoomObjectFactory objectFactory, IPlayer player, IPoint location = null);
    }
}
