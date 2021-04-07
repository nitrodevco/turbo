using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers
{
    public class FurnitureWiredTriggerEnterRoomLogic : FurnitureWiredTriggerLogic
    {
        public bool CanTrigger(IRoomObject roomObject)
        {
            if(RequiresPlayer)
            {
                if (roomObject.RoomObjectHolder is not IPlayer player) return false;
            }

            return true;
        }

        public override bool RequiresPlayer => true;
    }
}
