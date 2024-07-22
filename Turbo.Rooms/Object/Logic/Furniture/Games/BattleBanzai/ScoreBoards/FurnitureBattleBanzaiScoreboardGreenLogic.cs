using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.ScoreBoards;

[RoomObjectLogic("bb_score_g")]
public class FurnitureBattleBanzaiScoreboardGreenLogic : FurnitureBattleBanzaiScoreboardLogic
{
    public override GameTeamColor TeamColor => GameTeamColor.Green;
}