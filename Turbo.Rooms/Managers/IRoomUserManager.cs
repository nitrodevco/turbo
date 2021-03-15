using System;
using Turbo.Core;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Managers
{
    public interface IRoomUserManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IRoom Room { set; }
        public IRoomObject GetRoomObject(int id);
        public IRoomObject GetRoomObjectByUserId(int userId);
        public IRoomObject GetRoomObjectByUsername(string username);
        public IRoomObject AddRoomObject(IRoomObject roomObject, IPoint location, IPoint direction);
        public IRoomObject CreateRoomObjectAndAssign(IRoomObjectHolder roomObjectHolder, IPoint location, IPoint direction);
        public void EnterRoom();
        public void SendComposer(IComposer composer);
    }
}
