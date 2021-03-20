using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;
using Turbo.Core.Game;
using Turbo.Packets.Outgoing.Room.Engine;

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

            _room.SendComposer(new UserUpdateMessage
            {
                RoomObjects = updatedRoomObjects
            });
        }

        private void ProcessRoomObject(IRoomObject roomObject)
        {
            MovingAvatarLogic avatarLogic = (MovingAvatarLogic) roomObject.Logic;
            
            if (!avatarLogic.IsWalking) return; // or is rolling

            avatarLogic.ProcessNextLocation();

            if (avatarLogic.CurrentPath.Count == 0)
            {
                avatarLogic.StopWalking();

                return;
            }

            IPoint nextLocation = avatarLogic.CurrentPath[0];

            avatarLogic.CurrentPath.RemoveAt(0);

            CheckStep(roomObject, nextLocation);
        }

        private void CheckStep(IRoomObject roomObject, IPoint locationNext)
        {
            MovingAvatarLogic avatarLogic = (MovingAvatarLogic)roomObject.Logic;

            if (avatarLogic.NeedsRepathing)
            {
                avatarLogic.ClearPath();

                avatarLogic.WalkTo(avatarLogic.LocationGoal);

                ProcessRoomObject(roomObject);

                return;
            }

            if (locationNext == null)
            {
                avatarLogic.StopWalking();

                return;
            }

            bool isGoal = avatarLogic.CurrentPath.Count == 0;
            IRoomTile currentTile = avatarLogic.GetCurrentTile();
            IRoomTile nextTile = _room.RoomMap.GetTile(locationNext);

            if ((currentTile == null) || (nextTile == null) || (currentTile == nextTile))
            {
                avatarLogic.StopWalking();

                return;
            }

            double currentHeight = currentTile.GetWalkingHeight();
            double nextHeight = nextTile.GetWalkingHeight();

            if (Math.Abs(nextHeight - currentHeight) > Math.Abs(MAX_WALKING_HEIGHT))
            {
                avatarLogic.StopWalking();

                return;
            }

            if (isGoal)
            {
                if (!nextTile.IsOpen() || ((nextTile.Users.Count > 0) && !nextTile.Users.ContainsKey(roomObject.Id)))
                {
                    avatarLogic.StopWalking();

                    return;
                }
            }
            else
            {
                if ((nextTile.Users.Count > 0) && !nextTile.Users.ContainsKey(roomObject.Id))
                {
                    foreach (IRoomObject existingObject in nextTile.Users.Values)
                    {
                        MovingAvatarLogic existingAvatarLogic = (MovingAvatarLogic)existingObject.Logic;

                        if (!existingAvatarLogic.IsWalking)
                        {
                            avatarLogic.StopWalking();

                            return;
                        }

                        existingAvatarLogic.NeedsRepathing = true;
                    }

                    avatarLogic.NeedsRepathing = true;
                }

                if (!nextTile.IsOpen() || nextTile.CanSit() || nextTile.CanLay())
                {
                    avatarLogic.NeedsRepathing = true;
                }

                if (avatarLogic.NeedsRepathing)
                {
                    ProcessRoomObject(roomObject);

                    return;
                }
            }

            if (ALLOW_DIAGONALS)
            {
                bool isSideValid = _room.RoomMap.GetValidDiagonalTile(roomObject, new Point(nextTile.Location.X, currentTile.Location.Y)) != null;
                bool isOtherSideValid = _room.RoomMap.GetValidDiagonalTile(roomObject, new Point(currentTile.Location.X, nextTile.Location.Y)) != null;

                if (!isSideValid && !isOtherSideValid)
                {
                    avatarLogic.StopWalking();

                    return;
                }
            }

            if (currentTile.HighestObject != null)
            {
                if ((currentTile.HighestObject != nextTile.HighestObject) && (currentTile.HighestObject.Logic is FurnitureLogicBase furnitureLogic))
                {
                    furnitureLogic.OnLeave(roomObject);
                }
            }

            currentTile.RemoveRoomObject(roomObject);
            nextTile.AddRoomObject(roomObject);

            avatarLogic.RemoveStatus(RoomObjectAvatarStatus.Lay, RoomObjectAvatarStatus.Sit);
            avatarLogic.AddStatus(RoomObjectAvatarStatus.Move, nextTile.Location.X + "," + nextTile.Location.Y + "," + nextHeight);
            roomObject.Location.SetRotation(roomObject.Location.CalculateWalkDirection(locationNext));
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
