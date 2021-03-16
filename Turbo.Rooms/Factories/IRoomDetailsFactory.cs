using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;

namespace Turbo.Rooms.Factories
{
    public interface IRoomDetailsFactory
    {
        public IRoomDetails Create(RoomEntity roomEntity);
    }
}
