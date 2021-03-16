using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Cycles;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;
using Turbo.Core.Game;

namespace Turbo.Rooms.Cycles
{
    public class RoomUserStatusCycle : ICyclable
    {
        private static readonly int MAX_WALKING_HEIGHT = 2;
        private static readonly bool ALLOW_DIAGONALS = true;

        private readonly IRoom _room;

        public RoomUserStatusCycle(IRoom room)
        {
            _room = room;
        }

        public async Task Cycle()
        {
            IRoomUserManager roomUserManager = _room.RoomUserManager;

            if (roomUserManager == null) return;

            IList<IRoomObject> updatedRoomObjects = new List<IRoomObject>();

            IDictionary<int, IRoomObject> roomObjects = roomUserManager.RoomObjects;

            if (roomObjects.Count == 0) return;

            foreach (IRoomObject roomObject in roomObjects.Values)
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

            MovingAvatarLogic avatarLogic = (MovingAvatarLogic) roomObject.Logic;
            
            if (!avatarLogic.IsWalking) return; // or is rolling

            if (avatarLogic.CurrentPath.Count == 0)
            {
                avatarLogic.StopWalking();

                return;
            }

            avatarLogic.ProcessNextLocation();

            IPoint nextLocation = avatarLogic.CurrentPath[0];

            avatarLogic.CurrentPath.RemoveAt(0);

            CheckStep(roomObject, roomObject.Location, nextLocation);
        }

        private void CheckStep(IRoomObject roomObject, IPoint location, IPoint locationNext)
        {
            MovingAvatarLogic avatarLogic = (MovingAvatarLogic) roomObject.Logic;

            if ((location == null) || (locationNext == null))
            {
                avatarLogic.StopWalking();

                return;
            }

            bool isGoal = avatarLogic.CurrentPath.Count == 0;
            IRoomTile currentTile = _room.RoomMap.GetTile(location);
            IRoomTile nextTile = _room.RoomMap.GetValidTile(roomObject, locationNext, isGoal);

            if ((currentTile == null) || (currentTile == nextTile))
            {
                avatarLogic.StopWalking();

                return;
            }

            if (nextTile == null)
            {
                avatarLogic.ClearPath();

                avatarLogic.WalkTo(avatarLogic.LocationGoal);

                ProcessRoomObject(roomObject);

                return;
            }

            double currentHeight = currentTile.GetWalkingHeight();
            double nextHeight = nextTile.GetWalkingHeight();

            if (Math.Abs(nextHeight - currentHeight) > Math.Abs(MAX_WALKING_HEIGHT))
            {
                avatarLogic.StopWalking();

                return;
            }

            if (ALLOW_DIAGONALS && !location.Compare(locationNext))
            {
                bool isSideValid = _room.RoomMap.GetValidDiagonalTile(roomObject, new Point(locationNext.X, location.Y)) != null;
                bool isOtherSideValid = _room.RoomMap.GetValidDiagonalTile(roomObject, new Point(location.X, locationNext.Y)) != null;

                if (!isSideValid && !isOtherSideValid)
                {
                    avatarLogic.StopWalking();

                    return;
                }
            }

            if (isGoal)
            {
                if (!nextTile.IsOpen())
                {
                    avatarLogic.StopWalking();

                    return;
                }
            }
            else
            {
                if (!nextTile.IsOpen() || nextTile.CanSit() || nextTile.CanLay())
                {
                    avatarLogic.StopWalking();

                    return;
                }
            }

            currentTile.RemoveRoomObject(roomObject);
            nextTile.AddRoomObject(roomObject);

            if (currentTile.HighestObject != null)
            {
                if ((currentTile.HighestObject != nextTile.HighestObject) && (currentTile.HighestObject.Logic is FurnitureLogicBase furnitureLogic))
                {
                    furnitureLogic.OnLeave(roomObject);
                }
            }

            avatarLogic.RemoveStatus(RoomObjectAvatarStatus.Lay, RoomObjectAvatarStatus.Sit);
            avatarLogic.AddStatus(RoomObjectAvatarStatus.Move, nextTile.Location.X + "," + nextTile.Location.Y + "," + nextHeight);
            roomObject.Location.SetRotation(location.CalculateWalkDirection(locationNext));
            avatarLogic.LocationNext = locationNext;

            if (nextTile.HighestObject != null)
            {
                if (nextTile.HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    furnitureLogic.BeforeStep(roomObject);

                    if (nextTile.HighestObject != currentTile.HighestObject) furnitureLogic.OnEnter(roomObject);
                }
            }

            roomObject.NeedsUpdate = true;
        }
    }
}
