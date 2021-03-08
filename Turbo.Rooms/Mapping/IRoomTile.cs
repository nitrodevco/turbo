using System;
using System.Collections.Generic;
using Turbo.Rooms.Object;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public interface IRoomTile
    {
        public IPoint Location { get; }
        public double DefaultHeight { get; }

        public double Height { get; }
        public int RelativeHeight { get; }
        public RoomTileState State { get; }
        public IRoomObject HighestObject { get; }

        public Dictionary<int, IRoomObject> Users { get; }
        public Dictionary<int, IRoomObject> Furniture { get; }

        public bool IsDoor { get; }

        public void AddUser(IRoomObject roomObject);
        public void RemoveUser(IRoomObject roomObject);
        public void AddFurniture(IRoomObject roomObject);
        public void RemoveFurniture(IRoomObject roomObject);
        public void ResetTileHeight();

        public bool CanWalk();
        public bool CanSit();
        public bool CanLay();
        public bool CanStack();

    }
}
