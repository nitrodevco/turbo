using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;

namespace Turbo.Rooms.Factories;

public interface IRoomFactory
{
    public IRoom Create(RoomEntity roomEntity);
}