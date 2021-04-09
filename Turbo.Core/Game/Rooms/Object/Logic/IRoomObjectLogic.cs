using System;
using Turbo.Core.Game.Rooms.Mapping;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRoomObjectLogic : IDisposable, ICyclable
    {
        public bool OnReady();
        public bool SetRoomObject(IRoomObject roomObject);
        public IRoomTile GetCurrentTile();
    }
}
