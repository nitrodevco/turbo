using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureStackHelperLogic : FurnitureLogic
    {
        protected virtual void ResetHeight()
        {
            var tile = GetCurrentTile();

            if (tile == null) return;

            RoomObject.Location.Z = tile.DefaultHeight;

            NeedsSaving = true;
        }

        public override void OnPlace(IRoomManipulator roomManipulator)
        {
            base.OnPlace(roomManipulator);

            ResetHeight();
        }

        public override void OnMove(IRoomManipulator roomManipulator)
        {
            base.OnMove(roomManipulator);

            ResetHeight();
        }

        public override bool CanStack() => true;

        public override bool CanRoll() => false;

        public override bool CanToggle(IRoomObject roomObject) => false;

        public override bool IsOpen() => false;
    }
}
