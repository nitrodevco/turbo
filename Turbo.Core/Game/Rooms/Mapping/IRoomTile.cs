using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping
{
    public interface IRoomTile
    {
        public IPoint Location { get; }
        public double DefaultHeight { get; }

        public double Height { get; }
        public int RelativeHeight { get; }
        public RoomTileState State { get; }
        public IRoomObject HighestObject { get; }

        public bool HasStackHelper { get; }

        public IDictionary<int, IRoomObject> Users { get; }
        public IDictionary<int, IRoomObject> Furniture { get; }

        public bool IsDoor { get; set; }

        public void AddRoomObject(IRoomObject roomObject);
        public void RemoveRoomObject(IRoomObject roomObject);

        public void ResetTileHeight();
        public double GetWalkingHeight();
        public bool HasLogic(Type type);

        public bool IsOpen(IRoomObject roomObject = null);
        public bool CanWalk(IRoomObject roomObject = null);
        public bool CanSit(IRoomObject roomObject = null);
        public bool CanLay(IRoomObject roomObject = null);
        public bool CanStack();

    }
}
