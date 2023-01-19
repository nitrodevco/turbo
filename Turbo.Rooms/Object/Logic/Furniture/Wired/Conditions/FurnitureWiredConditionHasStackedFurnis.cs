using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions
{
    public class FurnitureWiredConditionHasStackedFurnis : FurnitureWiredConditionLogic
    {
        protected static int _anyFurni = 0;
        protected static int _allFurni = 1;

        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (!base.CanTrigger(wiredArguments)) return false;

            var matchType = _wiredData.IntParameters[0];

            if (matchType == _allFurni)
            {
                foreach (var floorObject in _selectedFloorObjects)
                {
                    if (!HasFurnitureAbove(floorObject)) return false;
                }

                return true;
            }

            else if (matchType == _anyFurni)
            {
                foreach (var floorObject in _selectedFloorObjects)
                {
                    if (!HasFurnitureAbove(floorObject)) continue;

                    return true;
                }
            }

            return false;
        }

        protected virtual bool HasFurnitureAbove(IRoomObjectFloor floorObject)
        {
            if (floorObject == null || floorObject.Disposed) return false;

            var roomTiles = floorObject.Logic.GetCurrentTiles();

            if (roomTiles.Count == 0) return false;

            foreach (var roomTile in roomTiles)
            {
                if (roomTile.GetFurnitureAbove(floorObject) == null) return false;
            }

            return true;
        }

        public override int WiredKey => (int)FurnitureWiredConditionType.HasStackedFurnis;
    }
}