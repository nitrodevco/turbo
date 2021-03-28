using System;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRoomObjectLogic : IDisposable
    {
        public bool OnReady();
        public bool SetRoomObject(IRoomObject roomObject);
        public void Cycle();
    }
}
