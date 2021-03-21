using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Managers
{
    public class RoomFurnitureManager : IRoomFurnitureManager
    {
        private IRoom _room;

        public RoomFurnitureManager(IRoom room)
        {
            _room = room;
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }
    }
}
