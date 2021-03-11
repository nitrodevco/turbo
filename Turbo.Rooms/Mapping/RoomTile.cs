using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

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
        private int _stackHelperHeight;

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
        }

        public void AddUser(IRoomObject roomObject)
        {
            if ((roomObject == null) || IsDoor) return;

            Users.Add(roomObject.Id, roomObject);
        }

        public void RemoveUser(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            Users.Remove(roomObject.Id);
        }

        public void AddFurniture(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            Furniture.Add(roomObject.Id, roomObject);

            // if ((furniture.height < this._tileHeight) && !(furniture.logic instanceof FurnitureStackHelperLogic)) return;

            ResetHighestObject();
        }

        public void RemoveFurniture(IRoomObject roomObject)
        {
            if (roomObject == null) return;

            Furniture.Remove(roomObject.Id);

            // if ((furniture !== this._highestItem) && !(furniture.logic instanceof FurnitureStackHelperLogic)) return;

            ResetHighestObject();
        }

        public void ResetTileHeight()
        {
            //Height = (HighestObject != null) ? HighestObject.Height : DefaultHeight;

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
                    int height = 0; // furniture height

                    //if() // logic is stack helper
                    //{
                    //    _hasstackhelper = true;
                    //    _stackhelperheight = height;
                    //    continue;
                    //}

                    if (height < Height) continue;

                    HighestObject = roomObject;
                    Height = height;
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

            //if(HighestObject != null)
            //{
            //    height = height;
            //}

            return height;
        }

        public bool CanWalk()
        {
            return true;
        }

        public bool CanSit()
        {
            return false;
        }

        public bool CanLay()
        {
            return false;
        }

        public bool CanStack()
        {
            return true;
        }
    }
}
