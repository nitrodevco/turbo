using Turbo.Core.Game.Furniture.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    [RoomObjectLogic("roller")]
    public class FurnitureRollerLogic : FurnitureFloorLogic
    {
        public override bool CanRoll() => false;

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;
    }
}
