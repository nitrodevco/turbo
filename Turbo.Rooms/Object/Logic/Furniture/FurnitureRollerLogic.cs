using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureRollerLogic : FurnitureLogic
    {
        public override bool CanStack() => false;

        public override bool CanRoll() => false;

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;
    }
}
