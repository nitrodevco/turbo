using System;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRoomObjectLogic : IDisposable
    {
        public bool SetRoomObject(IRoomObject roomObject);
    }
}
