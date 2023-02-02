using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Games.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture.Games
{
    public abstract class FurnitureTeamItemLogic : FurnitureFloorLogic
    {
        public abstract GameTeamColor TeamColor { get; }
    }
}