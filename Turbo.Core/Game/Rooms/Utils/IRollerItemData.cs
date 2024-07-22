using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Utils;

public interface IRollerItemData<T> where T : IRoomObject
{
    public T RoomObject { get; set; }
    public double Height { get; set; }
    public double HeightNext { get; set; }
}