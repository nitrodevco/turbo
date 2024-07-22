using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.Gates;

[RoomObjectLogic("bb_gate_g")]
public class FurnitureBattleBanzaiGateGreenLogic : FurnitureBattleBanzaiGateLogic
{
    public override GameTeamColor TeamColor => GameTeamColor.Green;
}