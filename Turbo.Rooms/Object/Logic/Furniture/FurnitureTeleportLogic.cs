using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureTeleportLogic : FurnitureLogic
    {
        public override void OnInteract(IRoomObject roomObject, int param)
        {
            if (roomObject.Logic is not IMovingAvatarLogic avatarLogic) return;
            
            IPoint goalPoint;

            if (IsOpen()) goalPoint = RoomObject.Location;
            else goalPoint = RoomObject.Location.GetPointForward();

            if (!roomObject.Location.Compare(goalPoint))
            {
                avatarLogic.GoalAction = OnInteract(roomObject, param);

                avatarLogic.WalkTo(goalPoint);

                return;
            }

            SetState(1);

            avatarLogic.CanWalk = false;
        }

        public override void OnPickup(IRoomManipulator roomManipulator)
        {
            SetState(0);

            base.OnPickup(roomManipulator);
        }
    }
}
