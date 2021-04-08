using System;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Furniture
{
    public interface IFurniture : IRoomObjectFurnitureHolder, IDisposable
    {
        public void Save();
        public bool SetRoom(IRoom room);
    }
}
