using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers
{
    public class FurnitureWiredTriggerEnterRoomLogic : FurnitureWiredTriggerLogic
    {
        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (!base.CanTrigger(wiredArguments)) return false;

            if (wiredArguments.UserObject == null) return false;

            string username = WiredData.StringParameter;

            if((username != null) && (username != ""))
            {
                if (wiredArguments.UserObject.RoomObjectHolder is not IRoomObjectUserHolder userHolder) return false;
                
                if (!username.Equals(userHolder.Name)) return false;
            }
            
            return true;
        }

        public override int WiredKey => (int)FurnitureWiredTriggerType.AvatarEntersRoom;
    }
}
