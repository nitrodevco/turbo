using Turbo.Core.Game.Furniture.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture;

[RoomObjectLogic("roller")]
public class FurnitureRollerLogic : FurnitureFloorLogic
{
    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;

    public override bool CanRoll()
    {
        return false;
    }
}