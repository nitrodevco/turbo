using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers
{
    [RoomObjectLogic("wf_trg_enter_room")]
    public class FurnitureWiredTriggerEnterRoomLogic : FurnitureWiredTriggerLogic
    {
        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (!base.CanTrigger(wiredArguments)) return false;

            if (wiredArguments.UserObject == null) return false;

            string username = WiredData.StringParameter;

            if ((username != null) && (username != ""))
            {
                if (wiredArguments.UserObject.RoomObjectHolder is not IRoomObjectAvatarHolder userHolder) return false;

                if (!username.Equals(userHolder.Name)) return false;
            }

            return true;
        }

        public override int WiredKey => (int)FurnitureWiredTriggerType.AvatarEntersRoom;
    }
}
