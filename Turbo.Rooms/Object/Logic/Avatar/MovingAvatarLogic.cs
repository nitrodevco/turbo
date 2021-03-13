using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Messages;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Avatar
{
    public class MovingAvatarLogic : RoomObjectLogicBase
    {
        public IDictionary<string, string> Statuses { get; private set; }

        public IPoint LocationNext { get; set; }
        public IPoint LocationGoal { get; private set; }
        public IList<IPoint> CurrentPath { get; private set; }
        
        public bool IsWalking { get; private set; }
        public bool CanWalk { get; private set; }

        public MovingAvatarLogic()
        {
            Statuses = new Dictionary<string, string>();
        }

        public override void ProcessUpdateMessage(RoomObjectUpdateMessage updateMessage)
        {
            base.ProcessUpdateMessage(updateMessage);

            if(updateMessage is RoomObjectAvatarWalkPathMessage walkPathMessage)
            {
                HandleRoomObjectAvatarWalkPath(walkPathMessage);

                return;
            }
        }

        private void HandleRoomObjectAvatarWalkPath(RoomObjectAvatarWalkPathMessage message)
        {
            if((message.Goal == null) || (message.Path == null) || (message.Path.Count == 0))
            {
                StopWalking();

                return;
            }

            LocationGoal = message.Goal;
            CurrentPath = message.Path;
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

            RemoveStatus(RoomObjectAvatarStatus.Move);

            RoomObject.NeedsUpdate = true;
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

            if(roomTile == null)
            {
                StopWalking();

                return;
            }

            if(roomTile.IsDoor)
            {
                RoomObject.Dispose();

                return;
            }

            UpdateHeight(roomTile);

            roomTile.OnStep(RoomObject);

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

            IRoomObject highestObject = roomTile.HighestObject;

            if(highestObject == null) // if the highest object cant sit / lay, clear these
            {
                Sit(false);
                Lay(false);
            }

            roomTile.OnStop(RoomObject);

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
            if((type == null) || (type.Length == 0)) return;

            Statuses.Add(type, value);

            RoomObject.NeedsUpdate = true;
        }

        public bool HasStatus(params string[] types)
        {
            if ((types == null) || (types.Length == 0)) return false;

            foreach (string type in types)
            {
                if (Statuses.ContainsKey(type)) return true;
            }

            return false;
        }

        public void RemoveStatus(params string[] types)
        {
            if((types == null) || (types.Length == 0)) return;

            foreach(string type in types)
            {
                Statuses.Remove(type);
            }

            RoomObject.NeedsUpdate = true;
        }

        public IRoomTile GetCurrentTile()
        {
            if ((RoomObject == null) || (RoomObject.Room == null) || (RoomObject.Room.RoomMap == null)) return null;

            return RoomObject.Room.RoomMap.GetTile(RoomObject.Location);
        }

        public IRoomTile GetNextTile()
        {
            if ((LocationNext == null) || (RoomObject == null) || (RoomObject.Room == null) || (RoomObject.Room.RoomMap == null)) return null;

            return RoomObject.Room.RoomMap.GetTile(LocationNext);
        }
    }
}
