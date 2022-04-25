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
        public IRoomObject HighestObject { get; private set; }

        public IDictionary<int, IRoomObject> Users { get; private set; }
        public IDictionary<int, IRoomObject> Furniture { get; private set; }

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

            Users = new Dictionary<int, IRoomObject>();
            Furniture = new Dictionary<int, IRoomObject>();

            ResetRelativeHeight();
        }

        public void AddRoomObject(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            if (roomObject.Logic is IMovingAvatarLogic)
            {
                if (IsDoor) return;

                if (Users.ContainsKey(roomObject.Id)) return;

                Users.Add(roomObject.Id, roomObject);

                return;
            }
            
            if (roomObject.Logic is IFurnitureLogic furnitureLogic)
            {
                Furniture.Add(roomObject.Id, roomObject);

                if ((furnitureLogic.Height < Height) && !(furnitureLogic is FurnitureStackHelperLogic)) return;

                ResetHighestObject();
            }
        }

        public void RemoveRoomObject(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            if (roomObject.Logic is IMovingAvatarLogic)
            {
                Users.Remove(roomObject.Id);

                return;
            }
            
            if (roomObject.Logic is IFurnitureLogic furnitureLogic)
            {
                Furniture.Remove(roomObject.Id);

                if ((roomObject != HighestObject) && !(furnitureLogic is FurnitureStackHelperLogic)) return;

                ResetHighestObject();

                return;
            }
        }

        public void ResetTileHeight()
        {
            Height = DefaultHeight;

            if (HighestObject != null && HighestObject.Logic is IFurnitureLogic furnitureLogic)
            {
                Height = furnitureLogic.Height;
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
                foreach (IRoomObject roomObject in Furniture.Values)
                {
                    if (roomObject.Logic is IFurnitureLogic furnitureLogic)
                    {
                        double height = furnitureLogic.Height;

                        if (furnitureLogic is FurnitureStackHelperLogic)
                        {
                            HasStackHelper = true;
                            _stackHelperHeight = height;

                            continue;
                        }

                        if (height < Height) continue;

                        HighestObject = roomObject;
                        Height = Math.Round((double)height, 3);
                    }
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
                if (HighestObject.Logic is IFurnitureLogic furnitureLogic && (furnitureLogic.CanSit() || furnitureLogic.CanLay()))
                {
                    height -= furnitureLogic.StackHeight;
                }
            }

            return Math.Round((double)height, 3);
        }

        public bool HasLogic(Type type)
        {
            foreach(IRoomObject roomObject in Furniture.Values)
            {
                if (roomObject.Logic.GetType() == type) return true;
            }

            return false;
        }

        public bool IsOpen(IRoomObject roomObject = null)
        {
            if (State == RoomTileState.Closed) return false;

            if (HighestObject != null && HighestObject.Logic is IFurnitureLogic furnitureLogic && !furnitureLogic.IsOpen(roomObject))
            {
                return false;
            }

            return true;
        }

        public bool CanWalk(IRoomObject roomObject = null)
        {
            if (HighestObject != null && HighestObject.Logic is IFurnitureLogic furnitureLogic)
            {
                if (!furnitureLogic.IsOpen(roomObject)) return false;

                if (HasStackHelper && (_stackHelperHeight >= furnitureLogic.Height)) return false;

                return true;
            }

            if (HasStackHelper) return false;

            return true;
        }

        public bool CanSit(IRoomObject roomObject = null)
        {
            if (HighestObject != null && HighestObject.Logic is IFurnitureLogic furnitureLogic && furnitureLogic.CanSit(roomObject)) return true;

            return false;
        }

        public bool CanLay(IRoomObject roomObject = null)
        {
            if (HighestObject != null && HighestObject.Logic is IFurnitureLogic furnitureLogic && furnitureLogic.CanLay(roomObject)) return true;

            return false;
        }

        public bool CanStack()
        {
            if (HighestObject != null && HighestObject.Logic is IFurnitureLogic furnitureLogic && !furnitureLogic.CanStack()) return false;

            return true;
        }
    }
}
