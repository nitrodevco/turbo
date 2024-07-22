using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Cycles;

public class RoomUserStatusCycle(IRoom _room) : RoomCycle(_room)
{
    public override async Task Cycle()
    {
        List<IRoomObjectAvatar> updatedAvatarObjects = new();

        foreach (var avatarObject in _room.RoomUserManager.AvatarObjects.RoomObjects.Values)
        {
            if (avatarObject.Disposed || !ProcessAvatarRoomObject(avatarObject) || !avatarObject.NeedsUpdate) continue;

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

    private bool ProcessAvatarRoomObject(IRoomObjectAvatar avatarObject)
    {
        if (avatarObject == null || avatarObject.Logic.IsRolling) return false;

        avatarObject.Logic.ProcessNextLocation();

        if (avatarObject.Disposed) return false;

        if (avatarObject.Logic.NeedsRepathing) avatarObject.Logic.ResetPath();

        if (avatarObject.Logic.IsWalking)
        {
            if (avatarObject.Logic.CurrentPath.Count == 0)
            {
                if (avatarObject.Logic.BeforeGoalAction != null)
                {
                    avatarObject.Logic.InvokeBeforeGoalAction();

                    if (avatarObject.Logic.CurrentPath.Count == 0)
                    {
                        avatarObject.Logic.StopWalking();

                        return true;
                    }
                }
                else
                {
                    avatarObject.Logic.StopWalking();

                    return true;
                }
            }

            var nextLocation = avatarObject.Logic.CurrentPath[0];

            avatarObject.Logic.CurrentPath.RemoveAt(0);

            CheckStep(avatarObject, nextLocation);
        }

        return true;
    }

    private void CheckStep(IRoomObjectAvatar avatarObject, IPoint locationNext)
    {
        if (locationNext == null)
        {
            avatarObject.Logic.StopWalking();

            return;
        }

        var isGoal = avatarObject.Logic.CurrentPath.Count == 0;
        var currentTile = avatarObject.Logic.GetCurrentTile();
        var nextTile = _room.RoomMap.GetTile(locationNext);

        if (currentTile == null || nextTile == null || currentTile == nextTile)
        {
            avatarObject.Logic.StopWalking();

            return;
        }

        var currentHeight = currentTile.GetWalkingHeight();
        var nextHeight = nextTile.GetWalkingHeight();

        if (Math.Abs(nextHeight - currentHeight) > Math.Abs(DefaultSettings.MaximumStepHeight))
        {
            avatarObject.Logic.StopWalking();

            return;
        }

        if (isGoal)
        {
            if (!nextTile.CanWalk(avatarObject) ||
                (nextTile.Avatars.Count > 0 && !nextTile.Avatars.Contains(avatarObject)))
            {
                avatarObject.Logic.StopWalking();

                return;
            }
        }
        else
        {
            if (nextTile.Avatars.Count > 0 && !nextTile.Avatars.Contains(avatarObject))
                if (!_room.RoomDetails.BlockingDisabled)
                    avatarObject.Logic.NeedsRepathing = true;

            if (!nextTile.CanWalk(avatarObject) || nextTile.CanSit(avatarObject) || nextTile.CanLay(avatarObject))
                avatarObject.Logic.NeedsRepathing = true;

            if (avatarObject.Logic.NeedsRepathing)
            {
                ProcessAvatarRoomObject(avatarObject);

                return;
            }
        }

        if (DefaultSettings.PathingAllowsDiagonals)
        {
            var isSideValid =
                _room.RoomMap.GetValidDiagonalTile(avatarObject,
                    new Point(nextTile.Location.X, currentTile.Location.Y)) != null;
            var isOtherSideValid =
                _room.RoomMap.GetValidDiagonalTile(avatarObject,
                    new Point(currentTile.Location.X, nextTile.Location.Y)) != null;

            if (!isSideValid && !isOtherSideValid)
            {
                avatarObject.Logic.StopWalking();

                return;
            }
        }

        if (currentTile.HighestObject != null)
            if (currentTile.HighestObject != nextTile.HighestObject)
                currentTile.HighestObject.Logic.OnLeave(avatarObject);

        currentTile.RemoveRoomObject(avatarObject);
        nextTile.AddRoomObject(avatarObject);

        avatarObject.Logic.RemoveStatus(RoomObjectAvatarStatus.Lay, RoomObjectAvatarStatus.Sit);
        avatarObject.Logic.AddStatus(RoomObjectAvatarStatus.Move,
            nextTile.Location.X + "," + nextTile.Location.Y + "," + nextHeight);
        avatarObject.Location.SetRotation(avatarObject.Location.CalculateWalkRotation(locationNext));
        avatarObject.Logic.LocationNext = locationNext;

        if (nextTile.HighestObject != null)
            if (nextTile.HighestObject != currentTile.HighestObject)
                nextTile.HighestObject.Logic.OnEnter(avatarObject);

        avatarObject.NeedsUpdate = true;
    }
}