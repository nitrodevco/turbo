using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Messages;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms
{
    public class RoomUserEngine
    {
        private IRoomManager _roomManager;

        public RoomUserEngine(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public void AvatarWalkTo(int roomId, int objectId, IPoint point)
        {
            IRoom room = _roomManager.GetOnlineRoom(roomId);

            if (room == null) return;

            IRoomObject roomObject = room.RoomUserManager.GetRoomObject(objectId);

            if (roomObject == null) return;

            point = point.Clone();

            // dispatch plugin event AvatarWalksEvent(room, roomObject, point)
            // if event cancelled, return

            // clear roller data
            // clear idle status

            // avatar process next point

            if (roomObject.Location.Compare(point)) return;

            IRoomTile goalTile = room.RoomMap.GetValidTile(roomObject, point);

            if(goalTile == null)
            {
                // avatar stop walking

                return;
            }

            IList<IPoint> path = room.RoomMap.PathFinder.MakePath(roomObject, goalTile.Location);

            roomObject.ProcessUpdateMessage(new RoomObjectAvatarWalkPathMessage(goalTile.Location, path));
        }
    }
}
