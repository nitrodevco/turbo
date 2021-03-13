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

        public IDictionary<int, IRoomObject> Users { get; }
        public IDictionary<int, IRoomObject> Furniture { get; }

        public bool IsDoor { get; }

        public void AddUser(IRoomObject roomObject);
        public void RemoveUser(IRoomObject roomObject);
        public void AddFurniture(IRoomObject roomObject);
        public void RemoveFurniture(IRoomObject roomObject);
        public void ResetTileHeight();

        public double GetWalkingHeight();

        public void OnEnter(IRoomObject roomObject);
        public void OnLeave(IRoomObject roomObject);
        public void BeforeStep(IRoomObject roomObject);
        public void OnStep(IRoomObject roomObject);
        public void OnStop(IRoomObject roomObject);

        public bool IsOpen();
        public bool CanWalk();
        public bool CanSit();
        public bool CanLay();
        public bool CanStack();

    }
}
