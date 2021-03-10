using System;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomUserManager : IRoomObjectContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public IRoomObject GetRoomObjectByUserId(int userId);
        public IRoomObject GetRoomObjectByUsername(string username);
    }
}
