using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureStackHelperLogic : FurnitureFloorLogic
    {
        public override bool CanStack() => false;

        public override bool CanRoll() => false;

        public override bool CanToggle(IRoomObjectAvatar avatar) => false;

        public override bool IsOpen(IRoomObjectAvatar avatar = null) => false;
    }
}
