using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions
{
    public class FurnitureWiredConditionNotTriggererOnFurni : FurnitureWiredConditionLogic
    {
        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (!base.CanTrigger(wiredArguments)) return false;

            var roomTile = wiredArguments.UserObject?.Logic?.GetCurrentTile();

            if (roomTile == null) return false;

            foreach (var floorObject in _selectedFloorObjects)
            {
                if (roomTile.HighestObject == floorObject) return false;
            }

            return true;
        }

        public override bool RequiresAvatar => true;

        public override int WiredKey => (int)FurnitureWiredConditionType.NotTriggererOnFurni;
    }
}