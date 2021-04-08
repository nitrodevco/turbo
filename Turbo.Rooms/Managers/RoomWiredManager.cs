using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Managers
{
    public class RoomWiredManager : IRoomWiredManager
    {
        private readonly IRoom _room;

        public RoomWiredManager(IRoom room)
        {
            _room = room;
        }
    }
}
