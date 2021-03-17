using System;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Managers
{
    public class RoomFurnitureManager : IRoomFurnitureManager
    {
        private IRoom _room;

        public IRoom Room { set => _room = value; }

        public RoomFurnitureManager()
        {
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }
    }
}
