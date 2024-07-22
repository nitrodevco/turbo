using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils;

public class RollerItemData<T> : IRollerItemData<T> where T : IRoomObject
{
    public T RoomObject { get; set; }
    public double Height { get; set; }
    public double HeightNext { get; set; }
}