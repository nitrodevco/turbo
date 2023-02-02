using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions
{
    [RoomObjectLogic("wf_cnd_not_hv_avtrs")]
    public class FurnitureWiredConditionNotFurniHasAvatars : FurnitureWiredConditionLogic
    {
        protected static int _anyFurni = 0;
        protected static int _allFurni = 1;

        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (!base.CanTrigger(wiredArguments)) return false;

            foreach (var floorObject in _selectedFloorObjects)
            {
                var roomTiles = floorObject.Logic.GetCurrentTiles();

                if (roomTiles.Count == 0) continue;

                foreach (var roomTile in roomTiles)
                {
                    if (roomTile != null && roomTile.Avatars.Count > 0) return false;
                }
            }

            return true;
        }

        public override int WiredKey => (int)FurnitureWiredConditionType.NotFurniHasAvatars;
    }
}