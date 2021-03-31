using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils
{
    public class RollerItemData : IRollerItemData
    {
        public IRoomObject RoomObject { get; set; }
        public double Height { get; set; }
        public double HeightNext { get; set; }
    }
}
