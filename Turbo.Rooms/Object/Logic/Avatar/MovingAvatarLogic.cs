using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Messages;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Avatar
{
    public class MovingAvatarLogic : RollingObjectLogic, IMovingAvatarLogic
    {
        public IDictionary<string, string> Statuses { get; private set; }

        public IPoint LocationNext { get; set; }
        public IPoint LocationGoal { get; private set; }
        public IList<IPoint> CurrentPath { get; private set; }
        
        public bool NeedsRepathing { get; set; }
        public bool IsWalking { get; private set; }
        public bool CanWalk { get; private set; }

        public MovingAvatarLogic()
        {
            Statuses = new Dictionary<string, string>();
        }

        public virtual void WalkTo(IPoint location)
        {
            NeedsRepathing = false;

            if (location == null) return;

            location = location.Clone();

            // clear roller data

            ProcessNextLocation();

            if (RoomObject.Location.Compare(location)) return;

            if(RoomObject.Room.RoomMap.GetValidTile(RoomObject, location) == null)
            {
                StopWalking();

                return;
            }

            IList<IPoint> path = RoomObject.Room.RoomMap.PathFinder.MakePath(RoomObject, location);

            WalkPath(location, path);
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
        }

        public void StopWalking()
        {
            if (!IsWalking) return;

            ClearWalking();

            InvokeCurrentLocation();
        }

        private void ClearWalking()
        {
            ClearPath();

            ProcessNextLocation();

            IsWalking = false;
            LocationNext = null;
            LocationGoal = null;

            if(RoomObject != null) RemoveStatus(RoomObjectAvatarStatus.Move);
        }

        public void ClearPath()
        {
            if (CurrentPath == null) return;

            CurrentPath.Clear();
        }

        public void ProcessNextLocation()
        {
            if ((RoomObject == null) || (LocationNext == null)) return;

            RoomObject.Location.X = LocationNext.X;
            RoomObject.Location.Y = LocationNext.Y;

            LocationNext = null;

            IRoomTile roomTile = GetCurrentTile();

            if(roomTile == null || roomTile.IsDoor)
            {
                StopWalking();

                if(roomTile.IsDoor) RoomObject.Dispose();

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

            if(!roomTile.CanSit() || !roomTile.CanLay())
            {
                Sit(false);
                Lay(false);
            }

            if((roomTile.HighestObject != null) && (roomTile.HighestObject.Logic is FurnitureLogicBase furnitureLogic))
            {
                furnitureLogic.OnStop(RoomObject);
            }

            UpdateHeight();
        }

        public virtual void Sit(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            RemoveStatus(RoomObjectAvatarStatus.Sit, RoomObjectAvatarStatus.Lay);

            if(flag)
            {
                rotation = (rotation == null) ? RoomObject.Location.CalculateSitDirection() : rotation;

                RoomObject.Location.SetRotation(rotation);

                AddStatus(RoomObjectAvatarStatus.Sit, string.Format("{0:N3}", height));
            }
        }

        public virtual void Lay(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            RemoveStatus(RoomObjectAvatarStatus.Lay, RoomObjectAvatarStatus.Sit);

            if (flag)
            {
                rotation = (rotation == null) ? RoomObject.Location.CalculateSitDirection() : rotation;

                RoomObject.Location.SetRotation(rotation);

                AddStatus(RoomObjectAvatarStatus.Lay, string.Format("{0:N3}", height));
            }
        }

        public void AddStatus(string type, string value)
        {
            if(string.IsNullOrWhiteSpace(type)) return;

            Statuses.Remove(type);
            Statuses.Add(type, value);

            RoomObject.NeedsUpdate = true;
        }

        public bool HasStatus(params string[] types) => types == null || types.Length == 0 ? false : types.Any(x => Statuses.ContainsKey(x));

        public void RemoveStatus(params string[] types)
        {
            if((types == null) || (types.Length == 0)) return;

            foreach(string type in types)
            {
                Statuses.Remove(type);
            }

            RoomObject.NeedsUpdate = true;
        }

        public IRoomTile GetCurrentTile() => RoomObject?.Room?.RoomMap?.GetTile(RoomObject.Location);

        public IRoomTile GetNextTile() => RoomObject?.Room?.RoomMap?.GetTile(LocationNext);
    }
}
