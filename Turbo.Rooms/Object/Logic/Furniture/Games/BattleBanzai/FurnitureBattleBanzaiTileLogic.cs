using Turbo.Core.Game.Furniture.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai;

[RoomObjectLogic("bb_patch1")]
public class FurnitureBattleBanzaiTileLogic : FurnitureFloorLogic
{
    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;
}