using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Cycles
{
    public class RoomObjectCycle : RoomCycle
    {
        public RoomObjectCycle(IRoom room) : base(room)
        {

        }

        public override async Task Cycle()
        {
            if (_room.RoomFurnitureManager != null)
            {
                IDictionary<int, IRoomObject> roomObjects = _room.RoomFurnitureManager.RoomObjects;

                if (roomObjects.Count > 0)
                {
                    foreach (IRoomObject roomObject in roomObjects.Values)
                    {
                        if (roomObject.Logic is not IRoomObjectLogic objectLogic) continue;

                        await objectLogic.Cycle();
                    }
                }
            }

            if (_room.RoomUserManager != null)
            {
                IDictionary<int, IRoomObject> roomObjects = _room.RoomUserManager.RoomObjects;

                if (roomObjects.Count > 0)
                {
                    foreach (IRoomObject roomObject in roomObjects.Values)
                    {
                        if (!(roomObject.Logic is IRoomObjectLogic objectLogic)) continue;

                        await objectLogic.Cycle();
                    }
                }
            }
        }
    }
}
