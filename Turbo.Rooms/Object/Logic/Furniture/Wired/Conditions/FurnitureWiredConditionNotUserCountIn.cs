using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions
{
    public class FurnitureWiredConditionNotUserCountIn : FurnitureWiredConditionLogic
    {
        protected static int _minUsers = 0;
        protected static int _maxUsers = 1;

        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (!base.CanTrigger(wiredArguments)) return false;

            if (_wiredData.IntParameters.Count != 2)
            {
                _wiredData.IntParameters.Clear();

                _wiredData.IntParameters.Insert(0, 1);
                _wiredData.IntParameters.Insert(1, 0);
            }

            var userCount = RoomObject?.Room?.RoomDetails?.UsersNow ?? 0;
            var minUsers = Math.Max(_wiredData.IntParameters[_minUsers], 1);
            var maxUsers = Math.Min(_wiredData.IntParameters[_maxUsers], 125);

            if (userCount < minUsers)
            {
                if ((maxUsers > 0) && (userCount > maxUsers)) return false;

                return true;
            }

            return false;
        }

        public override int WiredKey => (int)FurnitureWiredConditionType.NotUserCountIn;
    }
}