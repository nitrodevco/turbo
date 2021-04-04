using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureGateLogic : FurnitureLogic
    {
        private static readonly int _gateClosedState = 0;
        private static readonly int _gateOpenState = 1;

        public override void OnInteract(IRoomObject roomObject, int param)
        {
            IRoomTile roomTile = RoomObject.Room.RoomMap.GetTile(RoomObject.Location);

            if ((roomTile == null) || (roomTile.Users.Count > 0))  return;

            base.OnInteract(roomObject, param);
        }

        public override bool IsOpen(IRoomObject roomObject = null)
        {
            if (StuffData.GetState() == _gateClosedState) return false;

            if (StuffData.GetState() == _gateOpenState) return true;

            return base.IsOpen();
        }

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Controller;
    }
}
