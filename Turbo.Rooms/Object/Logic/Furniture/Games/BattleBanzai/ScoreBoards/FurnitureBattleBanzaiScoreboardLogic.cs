using Turbo.Core.Game.Furniture.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.ScoreBoards;

public abstract class FurnitureBattleBanzaiScoreboardLogic : FurnitureTeamItemLogic
{
    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;
}