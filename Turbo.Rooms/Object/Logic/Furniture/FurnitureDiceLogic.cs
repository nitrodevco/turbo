using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureDiceLogic : FurnitureLogic
    {
        private static readonly int _diceCycles = 4;
        private static readonly int _rollingState = -1;
        private static readonly int _closedState = 0;

        private int _remainingDiceCycles = -1;

        public override async Task Cycle()
        {
            if (_remainingDiceCycles > -1)
            {
                if (_remainingDiceCycles > 0)
                {
                    _remainingDiceCycles--;
                }

                if (_remainingDiceCycles != 0) return;

                _remainingDiceCycles = -1;

                SetState((int)Math.Floor(new Random().NextDouble() * (FurnitureDefinition.TotalStates - 1)) + 1);
            }
        }

        public virtual void ThrowDice(IRoomObject roomObject)
        {
            if (!CanToggle(roomObject)) return;

            SetState(_rollingState);

            _remainingDiceCycles = _diceCycles;
        }

        public virtual void DiceOff(IRoomObject roomObject)
        {
            if (!CanToggle(roomObject)) return;

            SetState(_closedState);

            _remainingDiceCycles = -1;
        }

        public override bool CanToggle(IRoomObject roomObject)
        {
            if (RoomObject.Location.GetDistanceAround(roomObject.Location) > 2) return false;

            Console.WriteLine("will toggle");

            return base.CanToggle(roomObject);
        }

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Everybody;
    }
}
