using System;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRoomObjectLogic : IDisposable, ICyclable
    {
        public bool OnReady();
        public bool SetRoomObject(IRoomObject roomObject);
    }
}
