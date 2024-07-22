using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.Gates;

public abstract class FurnitureBattleBanzaiGateLogic : FurnitureTeamItemLogic
{
    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;

    public override void OnStep(IRoomObjectAvatar roomObject)
    {
        base.OnStep(roomObject);
    }

    private void SetTeam(IRoomObjectAvatar avatar)
    {
        if (avatar == null) return;
    }
}