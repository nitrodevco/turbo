using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Rooms.Mapping
{
    public class RoomTile : IRoomTile
    {
        private static readonly int TILE_HEIGHT_DEFAULT = 32767;
        private static readonly int TILE_HEIGHT_MULTIPLIER = 256;

        public IPoint Location { get; private set; }
        public double DefaultHeight { get; private set; }

        public double Height { get; private set; }
        public int RelativeHeight { get; private set; }
        public RoomTileState State { get; private set; }
        public IRoomObjectFloor HighestObject { get; private set; }

        public IDictionary<int, IRoomObjectAvatar> Avatars { get; private set; }
        public IDictionary<int, IRoomObjectFloor> Furniture { get; private set; }

        public bool IsDoor { get; set; }
        public bool HasStackHelper { get; private set; }

        private double _stackHelperHeight;

        public RoomTile(IPoint location, double height, RoomTileState state)
        {
            Location = location;
            DefaultHeight = height;

            Height = DefaultHeight;
            RelativeHeight = TILE_HEIGHT_DEFAULT;
            State = state;

            Avatars = new Dictionary<int, IRoomObjectAvatar>();
            Furniture = new Dictionary<int, IRoomObjectFloor>();

            ResetRelativeHeight();
        }

        public void AddRoomObject(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            if (roomObject is IRoomObjectAvatar avatarObject)
            {
                if (IsDoor) return;

                if (Avatars.ContainsKey(roomObject.Id)) return;

                Avatars.Add(roomObject.Id, avatarObject);

                return;
            }

            if (roomObject is IRoomObjectFloor floorObject)
            {
                if (Furniture.ContainsKey(roomObject.Id)) return;

                Furniture.Add(roomObject.Id, floorObject);

                if ((floorObject.Logic.Height < Height) && floorObject.Logic is not FurnitureStackHelperLogic) return;

                ResetHighestObject();

                return;
            }
        }

        public void RemoveRoomObject(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            if (roomObject is IRoomObjectAvatar avatarObject)
            {
                Avatars.Remove(roomObject.Id);

                return;
            }

            if (roomObject is IRoomObjectFloor floorObject)
            {
                Furniture.Remove(roomObject.Id);

                if ((floorObject != HighestObject) && floorObject.Logic is not FurnitureStackHelperLogic) return;

                ResetHighestObject();

                return;
            }
        }

        public void ResetTileHeight()
        {
            Height = DefaultHeight;

            if (HighestObject != null)
            {
                Height = HighestObject.Logic.Height;
            }

            ResetRelativeHeight();
        }

        private void ResetHighestObject()
        {
            Height = DefaultHeight;
            HighestObject = null;

            HasStackHelper = false;
            _stackHelperHeight = 0;

            if (Furniture.Count > 0)
            {
                foreach (var floorObject in Furniture.Values)
                {
                    double height = floorObject.Logic.Height;

                    if (floorObject.Logic is FurnitureStackHelperLogic)
                    {
                        HasStackHelper = true;
                        _stackHelperHeight = height;

                        continue;
                    }

                    if (height < Height) continue;

                    HighestObject = floorObject;
                    Height = Math.Round((double)height, 3);
                }
            }

            ResetRelativeHeight();
        }

        private void ResetRelativeHeight()
        {
            RelativeHeight = TILE_HEIGHT_DEFAULT;

            if ((State == RoomTileState.Closed) || !CanStack()) return;

            RelativeHeight = (int)Math.Ceiling((decimal)(HasStackHelper ? _stackHelperHeight : Height) * TILE_HEIGHT_MULTIPLIER);
        }

        public double GetWalkingHeight()
        {
            double height = Height;

            if (HighestObject != null)
            {
                if (HighestObject.Logic.CanSit() || HighestObject.Logic.CanLay())
                {
                    height -= HighestObject.Logic.StackHeight;
                }
            }

            return Math.Round((double)height, 3);
        }

        public bool HasLogic(Type type)
        {
            foreach (var roomObject in Furniture.Values)
            {
                if (roomObject.Logic.GetType() == type) return true;
            }

            return false;
        }

        public bool IsOpen(IRoomObjectAvatar avatar = null)
        {
            if (State == RoomTileState.Closed) return false;

            if (HighestObject != null && !HighestObject.Logic.IsOpen(avatar))
            {
                return false;
            }

            return true;
        }

        public bool CanWalk(IRoomObjectAvatar avatar = null)
        {
            if (HighestObject != null)
            {
                if (!HighestObject.Logic.IsOpen(avatar)) return false;

                if (HasStackHelper && (_stackHelperHeight >= HighestObject.Logic.Height)) return false;

                return true;
            }

            if (HasStackHelper) return false;

            return true;
        }

        public bool CanSit(IRoomObjectAvatar avatar = null)
        {
            if (HighestObject != null && HighestObject.Logic.CanSit(avatar)) return true;

            return false;
        }

        public bool CanLay(IRoomObjectAvatar avatar = null)
        {
            if (HighestObject != null && HighestObject.Logic.CanLay(avatar)) return true;

            return false;
        }

        public bool CanStack()
        {
            if (HighestObject != null && !HighestObject.Logic.CanStack()) return false;

            return true;
        }
    }
}
