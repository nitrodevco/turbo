using Turbo.Core.Game.Furniture.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureRollerLogic : FurnitureFloorLogic
    {
        public override bool CanRoll() => false;

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;
    }
}
