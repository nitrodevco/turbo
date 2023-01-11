using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Cycles
{
    public class RoomUserStatusCycle : RoomCycle
    {
        private static readonly int MAX_WALKING_HEIGHT = 2;
        private static readonly bool ALLOW_DIAGONALS = true;

        public RoomUserStatusCycle(IRoom room) : base(room)
        {

        }

        public override async Task Cycle()
        {
            List<IRoomObjectAvatar> updatedAvatarObjects = new();

            var avatarObjects = _room.RoomUserManager.AvatarObjects.RoomObjects.Values;

            if (avatarObjects.Count == 0) return;

            foreach (var avatarObject in avatarObjects)
            {
                ProcessAvatarRoomObject(avatarObject);

                if (!avatarObject.NeedsUpdate) continue;

                avatarObject.NeedsUpdate = false;

                updatedAvatarObjects.Add(avatarObject);
            }

            if (updatedAvatarObjects.Count == 0) return;

            _room.SendComposer(new UserUpdateMessage
            {
                RoomObjects = updatedAvatarObjects
            });

            foreach (var avatarObject in updatedAvatarObjects)
            {
                avatarObject.Logic.RemoveStatus(RoomObjectAvatarStatus.Sign);

                avatarObject.NeedsUpdate = false;
            }
        }

        private void ProcessAvatarRoomObject(IRoomObjectAvatar avatarObject)
        {
            if (avatarObject == null) return;

            if (!avatarObject.Logic.IsWalking || avatarObject.Logic.IsRolling) return;

            avatarObject.Logic.ProcessNextLocation();

            if (avatarObject.Logic.NeedsRepathing) avatarObject.Logic.ResetPath();

            if (avatarObject.Logic.CurrentPath.Count == 0)
            {
                if (avatarObject.Logic.BeforeGoalAction != null)
                {
                    avatarObject.Logic.InvokeBeforeGoalAction();

                    if (avatarObject.Logic.CurrentPath.Count == 0)
                    {
                        avatarObject.Logic.StopWalking();

                        return;
                    }
                }
                else
                {
                    avatarObject.Logic.StopWalking();

                    return;
                }
            }

            IPoint nextLocation = avatarObject.Logic.CurrentPath[0];

            avatarObject.Logic.CurrentPath.RemoveAt(0);

            CheckStep(avatarObject, nextLocation);
        }

        private void CheckStep(IRoomObjectAvatar avatarObject, IPoint locationNext)
        {
            if (locationNext == null)
            {
                avatarObject.Logic.StopWalking();

                return;
            }

            bool isGoal = avatarObject.Logic.CurrentPath.Count == 0;
            IRoomTile currentTile = avatarObject.Logic.GetCurrentTile();
            IRoomTile nextTile = _room.RoomMap.GetTile(locationNext);

            if ((currentTile == null) || (nextTile == null) || (currentTile == nextTile))
            {
                avatarObject.Logic.StopWalking();

                return;
            }

            double currentHeight = currentTile.GetWalkingHeight();
            double nextHeight = nextTile.GetWalkingHeight();

            if (Math.Abs(nextHeight - currentHeight) > Math.Abs(MAX_WALKING_HEIGHT))
            {
                avatarObject.Logic.StopWalking();

                return;
            }

            if (isGoal)
            {
                if (!nextTile.IsOpen(avatarObject) || ((nextTile.Avatars.Count > 0) && !nextTile.Avatars.ContainsKey(avatarObject.Id)))
                {
                    avatarObject.Logic.StopWalking();

                    return;
                }
            }
            else
            {
                if ((nextTile.Avatars.Count > 0) && !nextTile.Avatars.ContainsKey(avatarObject.Id))
                {
                    if (!_room.RoomDetails.BlockingDisabled) avatarObject.Logic.NeedsRepathing = true;
                }

                if (!nextTile.IsOpen(avatarObject) || nextTile.CanSit(avatarObject) || nextTile.CanLay(avatarObject))
                {
                    avatarObject.Logic.NeedsRepathing = true;
                }

                if (avatarObject.Logic.NeedsRepathing)
                {
                    ProcessAvatarRoomObject(avatarObject);

                    return;
                }
            }

            if (ALLOW_DIAGONALS)
            {
                bool isSideValid = _room.RoomMap.GetValidDiagonalTile(avatarObject, new Point(nextTile.Location.X, currentTile.Location.Y)) != null;
                bool isOtherSideValid = _room.RoomMap.GetValidDiagonalTile(avatarObject, new Point(currentTile.Location.X, nextTile.Location.Y)) != null;

                if (!isSideValid && !isOtherSideValid)
                {
                    avatarObject.Logic.StopWalking();

                    return;
                }
            }

            if (currentTile.HighestObject != null)
            {
                if (currentTile.HighestObject != nextTile.HighestObject)
                {
                    currentTile.HighestObject.Logic.OnLeave(avatarObject);
                }
            }

            currentTile.RemoveRoomObject(avatarObject);
            nextTile.AddRoomObject(avatarObject);

            avatarObject.Logic.RemoveStatus(RoomObjectAvatarStatus.Lay, RoomObjectAvatarStatus.Sit);
            avatarObject.Logic.AddStatus(RoomObjectAvatarStatus.Move, nextTile.Location.X + "," + nextTile.Location.Y + "," + nextHeight);
            avatarObject.Location.SetRotation(avatarObject.Location.CalculateWalkRotation(locationNext));
            avatarObject.Logic.LocationNext = locationNext;

            if (nextTile.HighestObject != null)
            {
                nextTile.HighestObject.Logic.BeforeStep(avatarObject);

                if (nextTile.HighestObject != currentTile.HighestObject) nextTile.HighestObject.Logic.OnEnter(avatarObject);
            }

            avatarObject.NeedsUpdate = true;
        }
    }
}
