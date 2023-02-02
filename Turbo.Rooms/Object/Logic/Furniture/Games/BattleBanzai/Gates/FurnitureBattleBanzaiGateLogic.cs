using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.Gates
{
    public abstract class FurnitureBattleBanzaiGateLogic : FurnitureTeamItemLogic
    {
        public override void OnStep(IRoomObjectAvatar roomObject)
        {
            base.OnStep(roomObject);
        }

        private void SetTeam(IRoomObjectAvatar avatar)
        {
            if (avatar == null) return;
        }

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;
    }
}