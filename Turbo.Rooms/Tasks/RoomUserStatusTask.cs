using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Tasks
{
    public class RoomUserStatusTask
    {
        private static readonly int MAX_WALKING_HEIGHT = 2;
        private static readonly bool ALLOW_DIAGONALS = true;

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

            if (!(roomObject.Logic is MovingAvatarLogic)) return;

            MovingAvatarLogic logic = (MovingAvatarLogic) roomObject.Logic;

            if(!logic.IsWalking || (logic.CurrentPath.Count == 0))
            {
                logic.StopWalking();

                return;
            }

            logic.ProcessNextLocation();

            IPoint nextLocation = logic.CurrentPath[0];

            logic.CurrentPath.RemoveAt(0);

            CheckStep(roomObject, roomObject.Location, nextLocation);
        }

        private void CheckStep(IRoomObject roomObject, IPoint location, IPoint locationNext)
        {
            if (roomObject == null) return;

            MovingAvatarLogic movingAvatarLogic = (MovingAvatarLogic) roomObject.Logic;

            if ((location == null) || (locationNext == null))
            {
                movingAvatarLogic.StopWalking();

                return;
            }

            bool isGoal = movingAvatarLogic.CurrentPath.Count == 0;
            IRoomTile currentTile = movingAvatarLogic.GetCurrentTile();
            IRoomTile nextTile = movingAvatarLogic.GetNextTile();

            if((currentTile == null) || (currentTile == nextTile))
            {
                movingAvatarLogic.StopWalking();

                return;
            }

            if(nextTile == null)
            {
                movingAvatarLogic.ClearPath();

                // try path finding the goal again

                ProcessRoomObject(roomObject);

                return;
            }

            double currentHeight = currentTile.GetWalkingHeight();
            double nextHeight = nextTile.GetWalkingHeight();

            if(Math.Abs(nextHeight - currentHeight) > Math.Abs(MAX_WALKING_HEIGHT))
            {
                movingAvatarLogic.StopWalking();

                return;
            }

            if(ALLOW_DIAGONALS && !location.Compare(locationNext))
            {
                bool isSideValid = (roomObject.Room.RoomMap.GetValidDiagonalTile(roomObject, new Point(locationNext.X, location.Y)) != null);
                bool isOtherSideValid = (roomObject.Room.RoomMap.GetValidDiagonalTile(roomObject, new Point(location.X, locationNext.Y)) != null);

                if(!isSideValid && !isOtherSideValid)
                {
                    movingAvatarLogic.StopWalking();

                    return;
                }
            }

            IRoomObject currentHighestObject = currentTile.HighestObject;
            IRoomObject nextHighestObject = nextTile.HighestObject;

            if(nextHighestObject != null)
            {
                if(isGoal)
                {
                    // if highestobject is not open, stop walking return
                }
                else
                {
                    // if highestobject is not open, or the highestobject can sit/lay stop walking return
                }
            }

            currentTile.RemoveUser(roomObject);
            nextTile.AddUser(roomObject);

            if(currentHighestObject != null && (currentHighestObject != nextHighestObject))
            {
                // current highest object onleave
            }

            movingAvatarLogic.RemoveStatus(RoomObjectAvatarStatus.Lay, RoomObjectAvatarStatus.Sit);
            movingAvatarLogic.AddStatus(RoomObjectAvatarStatus.Move, nextTile.Location.X + "," + nextTile.Location.Y + "," + nextHeight);

            roomObject.Location.SetRotation(location.CalculateWalkDirection(locationNext));

            movingAvatarLogic.LocationNext = locationNext;

            nextTile.BeforeStep(roomObject);

            if(nextHighestObject != null)
            {

            }

            roomObject.NeedsUpdate = true;
        }
    }
}
