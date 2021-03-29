using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureStackHelperLogic : FurnitureLogic
    {
        public override bool CanStack() => false;

        public override bool CanRoll() => false;

        public override bool CanToggle(IRoomObject roomObject) => false;

        public override bool IsOpen() => false;
    }
}
