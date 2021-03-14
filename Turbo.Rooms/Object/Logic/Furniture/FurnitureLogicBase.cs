using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureLogicBase : RoomObjectLogicBase, IFurnitureLogic
    {
        public void OnEnter(IRoomObject roomObject)
        {
            return;
        }

        public void OnLeave(IRoomObject roomObject)
        {
            return;
        }

        public void BeforeStep(IRoomObject roomObject)
        {
            return;
        }

        public void OnStep(IRoomObject roomObject)
        {
            return;
        }

        public void OnStop(IRoomObject roomObject)
        {
            return;
        }

        public void OnInteract(IRoomObject roomObject)
        {
            return;
        }

        public void OnPlace(IRoomManipulator roomManipulator)
        {
            return;
        }

        public void OnMove(IRoomManipulator roomManipulator)
        {
            return;
        }

        public void OnPickup(IRoomManipulator roomManipulator)
        {
            return;
        }

        public bool CanStack()
        {
            return false;
        }

        public bool CanWalk()
        {
            return false;
        }

        public bool CanSit()
        {
            return false;
        }

        public bool CanLay()
        {
            return false;
        }

        public bool CanRoll()
        {
            return false;
        }

        public bool CanToggle()
        {
            return false;
        }

        public bool IsOpen()
        {
            return false;
        }

        public double StackHeight()
        {
            return 0;
        }

        public double Height
        {
            get
            {
                return RoomObject.Location.Z + StackHeight();
            }
        }
    }
}
