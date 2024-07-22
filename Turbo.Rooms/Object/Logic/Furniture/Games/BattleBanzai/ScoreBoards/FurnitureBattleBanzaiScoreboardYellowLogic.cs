using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.ScoreBoards;

[RoomObjectLogic("bb_score_y")]
public class FurnitureBattleBanzaiScoreboardYellowLogic : FurnitureBattleBanzaiScoreboardLogic
{
    public override GameTeamColor TeamColor => GameTeamColor.Yellow;
}