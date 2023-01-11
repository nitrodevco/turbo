using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectFloor : IRoomObject
    {
        public IRoomObjectFloorHolder RoomObjectHolder { get; }
        public IFurnitureFloorLogic Logic { get; }
        public IPoint Location { get; }

        public bool SetHolder(IRoomObjectFloorHolder roomObjectHolder);
        public void SetLogic(IFurnitureFloorLogic logic);
        public void SetLocation(IPoint point);
    }
}
