using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Rooms.Mapping
{
    public class RoomTile : IRoomTile
    {
        private static readonly int TILE_HEIGHT_DEFAULT = 32767;
        private static readonly int TILE_HEIGHT_MULTIPLIER = 256;

        private readonly IRoom _room;

        public IPoint Location { get; private set; }
        public double DefaultHeight { get; private set; }

        public double Height { get; private set; }
        public int RelativeHeight { get; private set; }
        public RoomTileState State { get; private set; }
        public IRoomObject HighestObject { get; private set; }

        public IDictionary<int, IRoomObject> Users { get; private set; }
        public IDictionary<int, IRoomObject> Furniture { get; private set; }
    
        public bool IsDoor { get; set; }

        private bool _hasStackHelper;
        private double _stackHelperHeight;

        public RoomTile(IRoom room, IPoint location, double height, RoomTileState state)
        {
            _room = room;

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

            if(roomObject.Logic is AvatarLogic avatarLogic)
            {
                if (IsDoor) return;

                if (Users.ContainsKey(roomObject.Id)) return;

                Users.Add(roomObject.Id, roomObject);
            }
            else if(roomObject.Logic is FurnitureLogicBase furnitureLogic)
            {
                Furniture.Add(roomObject.Id, roomObject);

                if ((furnitureLogic.Height < Height) && !(furnitureLogic is FurnitureStackHelperLogic)) return;

                ResetHighestObject();
            }
        }

        public void RemoveRoomObject(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            if (roomObject.Logic is AvatarLogic avatarLogic)
            {
                Users.Remove(roomObject.Id);
            }
            else if (roomObject.Logic is FurnitureLogicBase furnitureLogic)
            {
                Furniture.Remove(roomObject.Id);

                if ((roomObject != HighestObject) && !(furnitureLogic is FurnitureStackHelperLogic)) return;

                ResetHighestObject();
            }
        }

        public void ResetTileHeight()
        {
            Height = DefaultHeight;

            if(HighestObject != null)
            {
                if (HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    Height = furnitureLogic.Height;
                }
            }

            ResetRelativeHeight();
        }

        private void ResetHighestObject()
        {
            Height = DefaultHeight;
            HighestObject = null;

            _hasStackHelper = false;
            _stackHelperHeight = 0;

            if(Furniture.Count > 0)
            {
                foreach(IRoomObject roomObject in Furniture.Values)
                {
                    if (roomObject.Logic is FurnitureLogicBase furnitureLogic)
                    {
                        double height = furnitureLogic.Height;

                        if(furnitureLogic is FurnitureStackHelperLogic)
                        {
                            _hasStackHelper = true;
                            _stackHelperHeight = height;

                            continue;
                        }

                        if (height < Height) continue;

                        HighestObject = roomObject;
                        Height = height;
                    }
                }
            }

            ResetRelativeHeight();
        }

        private void ResetRelativeHeight()
        {
            RelativeHeight = TILE_HEIGHT_DEFAULT;

            if ((State == RoomTileState.Closed) || !CanStack()) return;

            RelativeHeight = (int) Math.Ceiling((decimal) (_hasStackHelper ? _stackHelperHeight : Height) * TILE_HEIGHT_MULTIPLIER);
        }

        public double GetWalkingHeight()
        {
            double height = Height;

            if (HighestObject != null)
            {
                if (HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    if (furnitureLogic.CanSit() || furnitureLogic.CanLay()) height -= furnitureLogic.StackHeight();
                }
            }

            return height;
        }

        public bool IsOpen()
        {
            if (State == RoomTileState.Closed) return false;

            if (HighestObject != null)
            {
                if (HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    if (!furnitureLogic.IsOpen()) return false;
                }
            }

            return true;
        }

        public bool CanWalk()
        {
            if(HighestObject != null)
            {
                if(HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    if (!furnitureLogic.IsOpen()) return false;

                    if (_hasStackHelper && (_stackHelperHeight >= furnitureLogic.Height)) return false;

                    return true;
                }
            }

            if (_hasStackHelper) return false;

            return true;
        }

        public bool CanSit()
        {
            if (HighestObject != null)
            {
                if (HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    if (furnitureLogic.CanSit()) return true;
                }
            }

            return false;
        }

        public bool CanLay()
        {
            if (HighestObject != null)
            {
                if (HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    if (furnitureLogic.CanLay()) return true;
                }
            }

            return false;
        }

        public bool CanStack()
        {
            if (HighestObject != null)
            {
                if (HighestObject.Logic is FurnitureLogicBase furnitureLogic)
                {
                    if (!furnitureLogic.CanStack()) return false;
                }
            }

            return true;
        }
    }
}
