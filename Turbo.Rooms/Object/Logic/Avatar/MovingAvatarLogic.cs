using System;
using System.Collections.Generic;
using System.Linq;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Rooms.Object.Logic.Avatar
{
    public class MovingAvatarLogic : RollingObjectLogic, IMovingAvatarLogic
    {
        public IDictionary<string, string> Statuses { get; private set; }

        public IPoint LocationNext { get; set; }
        public IPoint LocationGoal { get; private set; }
        public IList<IPoint> CurrentPath { get; private set; }
        public Action<IRoomObject> BeforeGoalAction { get; set; }
        public Action<IRoomObject> GoalAction { get; set; }

        public bool NeedsRepathing { get; set; }
        public bool IsWalking { get; private set; }
        public bool CanWalk { get; set; }

        public MovingAvatarLogic()
        {
            Statuses = new Dictionary<string, string>();
        }

        public virtual void WalkTo(IPoint location, bool selfWalk = false)
        {
            NeedsRepathing = false;

            if (location == null) return;

            if (!CanWalk && selfWalk) return;

            location = location.Clone();

            RollerData = null;

            ProcessNextLocation();

            if (RoomObject.Location.Compare(location)) return;

            IRoomTile roomTile = RoomObject.Room.RoomMap.GetValidTile(RoomObject, location);

            if (roomTile == null)
            {
                StopWalking();

                return;
            }

            if (roomTile.CanLay() && roomTile.HighestObject != null)
            {
                location = RoomObject.Room.RoomMap.GetValidPillowPoint(RoomObject, roomTile.HighestObject, location);

                if (location == null)
                {
                    StopWalking();

                    return;
                }
            }

            IList<IPoint> path = RoomObject.Room.RoomMap.PathFinder.MakePath(RoomObject, location);

            WalkPath(location, path);
        }

        public virtual void GoTo(IPoint location, bool selfWalk = false)
        {
            NeedsRepathing = false;

            if (location == null) return;

            if (!CanWalk && selfWalk) return;

            location = location.Clone();

            RollerData = null;

            if (RoomObject.Location.Compare(location)) return;

            StopWalking();

            LocationNext = location;

            IRoomTile currentTile = GetCurrentTile();
            IRoomTile nextTile = GetNextTile();

            if (currentTile == null || nextTile == null) return;

            if(currentTile.HighestObject != null && currentTile.HighestObject.Logic is IFurnitureLogic currentLogic)
            {
                if (currentTile.HighestObject != nextTile.HighestObject) currentLogic.OnLeave(RoomObject);
            }

            currentTile.RemoveRoomObject(RoomObject);
            nextTile.AddRoomObject(RoomObject);

            if (nextTile.HighestObject != null && nextTile.HighestObject.Logic is IFurnitureLogic nextLogic)
            {
                if (nextTile.HighestObject != currentTile.HighestObject) nextLogic.OnLeave(RoomObject);
            }

            ProcessNextLocation();
            InvokeCurrentLocation();

            RoomObject.Location.SetRotation(location.Rotation);
        }

        private void WalkPath(IPoint goal, IList<IPoint> path)
        {
            if ((goal == null) || (path == null) || (path.Count == 0))
            {
                StopWalking();

                return;
            }

            LocationGoal = goal;
            CurrentPath = path;
            IsWalking = true;

            BeforeGoalAction = null;
            GoalAction = null;
        }

        public virtual void ResetPath()
        {
            if (LocationGoal == null) return;

            ClearPath();
            WalkTo(LocationGoal, true);
        }

        public void StopWalking()
        {
            if (!IsWalking) return;

            ClearWalking();

            InvokeCurrentLocation();

            InvokeGoalAction();
        }

        private void ClearWalking()
        {
            ClearPath();

            ProcessNextLocation();

            IsWalking = false;
            LocationNext = null;
            LocationGoal = null;

            if (RoomObject != null) RemoveStatus(RoomObjectAvatarStatus.Move);
        }

        public void ClearPath()
        {
            if (CurrentPath == null) return;

            CurrentPath.Clear();
            LocationNext = null;
        }

        public void ProcessNextLocation()
        {
            if ((RoomObject == null) || (LocationNext == null)) return;

            RoomObject.Location.X = LocationNext.X;
            RoomObject.Location.Y = LocationNext.Y;

            LocationNext = null;

            IRoomTile roomTile = GetCurrentTile();

            if (roomTile == null || roomTile.IsDoor)
            {
                StopWalking();

                if (roomTile.IsDoor) RoomObject.Dispose();

                return;
            }

            UpdateHeight(roomTile);

            if ((roomTile.HighestObject != null) && (roomTile.HighestObject.Logic is FurnitureLogicBase furnitureLogic))
            {
                furnitureLogic.OnStep(RoomObject);
            }

            RoomObject.NeedsUpdate = true;
        }

        public void UpdateHeight(IRoomTile roomTile = null)
        {
            roomTile ??= GetCurrentTile();

            if (roomTile == null) return;

            double height = roomTile.GetWalkingHeight();
            double oldHeight = RoomObject.Location.Z;

            if (height == oldHeight) return;

            RoomObject.Location.Z = height;

            RoomObject.NeedsUpdate = true;
        }

        public void InvokeCurrentLocation()
        {
            IRoomTile roomTile = GetCurrentTile();

            if (roomTile == null) return;

            if (!roomTile.CanSit() || !roomTile.CanLay())
            {
                Sit(false);
                Lay(false);
            }

            if ((roomTile.HighestObject != null) && (roomTile.HighestObject.Logic is FurnitureLogicBase furnitureLogic))
            {
                furnitureLogic.OnStop(RoomObject);
            }

            UpdateHeight();
        }

        public void InvokeBeforeGoalAction()
        {
            if (BeforeGoalAction == null) return;

            BeforeGoalAction(RoomObject);

            BeforeGoalAction = null;
        }

        public void InvokeGoalAction()
        {
            if (GoalAction == null) return;

            GoalAction(RoomObject);

            GoalAction = null;
        }

        public virtual void Sit(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if (flag)
            {
                RemoveStatus(RoomObjectAvatarStatus.Lay);

                rotation = (rotation == null) ? RoomObject.Location.CalculateSitRotation() : rotation;

                RoomObject.Location.SetRotation(rotation);

                AddStatus(RoomObjectAvatarStatus.Sit, string.Format("{0:N3}", height));
            }
            else
            {
                RemoveStatus(RoomObjectAvatarStatus.Sit);
            }
        }

        public virtual void Lay(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if (flag)
            {
                RemoveStatus(RoomObjectAvatarStatus.Sit);

                rotation = (rotation == null) ? RoomObject.Location.CalculateSitRotation() : rotation;

                RoomObject.Location.SetRotation(rotation);

                AddStatus(RoomObjectAvatarStatus.Lay, string.Format("{0:N3}", height));
            }
            else
            {
                RemoveStatus(RoomObjectAvatarStatus.Lay);
            }
        }

        public void AddStatus(string type, string value)
        {
            if (string.IsNullOrWhiteSpace(type)) return;

            Statuses.Remove(type);
            Statuses.Add(type, value);

            RoomObject.NeedsUpdate = true;
        }

        public bool HasStatus(params string[] types) => types != null && types.Length != 0 && types.Any(x => Statuses.ContainsKey(x));

        public void RemoveStatus(params string[] types)
        {
            if ((types == null) || (types.Length == 0)) return;

            bool updated = false;

            foreach (string type in types)
            {
                if (!HasStatus(type)) continue;

                Statuses.Remove(type);

                updated = true;
            }

            if(updated) RoomObject.NeedsUpdate = true;
        }

        public IRoomTile GetNextTile() => RoomObject?.Room?.RoomMap?.GetTile(LocationNext);
    }
}
