using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Game.Rooms.Object;

public interface IRoomObjectHolder<T> where T : IRoomObject
{
    public T RoomObject { get; }

    public int Id { get; }
    public RoomObjectHolderType Type { get; }

    public Task<bool> SetupRoomObject();
    public bool SetRoomObject(T roomObject);
    public void ClearRoomObject();
}