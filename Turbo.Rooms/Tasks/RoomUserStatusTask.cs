using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Tasks
{
    public class RoomUserStatusTask
    {
        private readonly IRoom _room;

        public RoomUserStatusTask(IRoom room)
        {
            _room = room;
        }

        public void Run()
        {
            IRoomUserManager roomUserManager = _room.RoomUserManager;

            if (roomUserManager == null) return;

            IList<IRoomObject> updatedRoomObjects = new List<IRoomObject>();

            IDictionary<int, IRoomObject> roomObjects = roomUserManager.RoomObjects;

            if (roomObjects.Count == 0) return;

            foreach(IRoomObject roomObject in roomObjects.Values)
            {
                if (!(roomObject.Logic is MovingAvatarLogic)) continue;

                ProcessRoomObject(roomObject);

                if (!roomObject.NeedsUpdate) continue;

                roomObject.NeedsUpdate = false;

                updatedRoomObjects.Add(roomObject);
            }

            if (updatedRoomObjects.Count == 0) return;

            // COMPOSER: USER STATUS UPDATE
        }

        private void ProcessRoomObject(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            MovingAvatarLogic movingAvatarLogic = (MovingAvatarLogic) roomObject.Logic;

        }

        private void CheckStep(IRoomObject roomObject, IPoint location, IPoint locationNext)
        {
            if (roomObject == null) return;

            MovingAvatarLogic movingAvatarLogic = (MovingAvatarLogic) roomObject.Logic;



            if ((location == null) || (locationNext == null))
            {

            }

            if((roomObject == null) || (location == null) || (locationNext == null))
            {

            }
        }
    }
}
