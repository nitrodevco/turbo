using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomUserManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IRoomObjectContainer<IRoomObjectAvatar> AvatarObjects { get; }
        public IRoomObjectAvatar GetRoomObjectByUserId(int userId);
        public IRoomObjectAvatar GetRoomObjectByUsername(string username);
        public IRoomObjectAvatar EnterRoom(IPlayer player, IPoint location = null);
    }
}
