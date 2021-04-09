using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers
{
    public class FurnitureWiredTriggerEnterRoomLogic : FurnitureWiredTriggerLogic
    {
        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (wiredArguments.RoomObject == null) return false;
            
            return true;
        }
    }
}
