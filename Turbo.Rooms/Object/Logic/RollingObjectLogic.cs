using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic
{
    public class RollingObjectLogic : RoomObjectLogicBase, IRollingObjectLogic
    {
        private IRollerData _rollerData;

        public bool IsRolling => RollerData != null;

        public override void Dispose()
        {
            RollerData = null;

            base.Dispose();
        }

        public IRollerData RollerData
        {
            get => _rollerData;
            set
            {
                if(_rollerData != null)
                {
                    _rollerData.RemoveRoomObject(RoomObject);
                }

                _rollerData = value;
            }
        }
    }
}
