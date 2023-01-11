using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object
{
    public class RoomObjectFloor : RoomObject, IRoomObjectFloor
    {
        public IRoomObjectFloorHolder RoomObjectHolder { get; protected set; }
        public IFurnitureFloorLogic Logic { get; protected set; }
        public IPoint Location { get; private set; }

        private IRoomObjectContainer<IRoomObjectFloor> _roomObjectContainer;

        public RoomObjectFloor(IRoom room, IRoomObjectContainer<IRoomObjectFloor> roomObjectContainer, int id) : base(room, id)
        {
            Location = new Point();

            _roomObjectContainer = roomObjectContainer;
        }

        protected override void OnDispose()
        {
            if (_roomObjectContainer != null) _roomObjectContainer.RemoveRoomObject(Id);

            SetLogic(null);

            if (RoomObjectHolder != null)
            {
                RoomObjectHolder.ClearRoomObject();

                RoomObjectHolder = null;
            }

            _roomObjectContainer = null;
        }

        public virtual bool SetHolder(IRoomObjectFloorHolder roomObjectHolder)
        {
            if (roomObjectHolder == null) return false;

            RoomObjectHolder = roomObjectHolder;

            return true;
        }

        public virtual void SetLogic(IFurnitureFloorLogic logic)
        {
            if (logic == Logic) return;

            var currentLogic = Logic;

            if (currentLogic != null)
            {
                Logic = null;

                currentLogic.SetRoomObject(null);
            }

            Logic = logic;

            if (Logic != null)
            {
                Logic.SetRoomObject(this);
            }
        }

        public virtual void SetLocation(IPoint point)
        {
            if (point == null) return;

            if ((point.X == Location.X) && (point.Y == Location.Y) && (point.Z == Location.Z) && (point.Rotation == Location.Rotation) && (point.HeadRotation == Location.HeadRotation)) return;

            Location.X = point.X;
            Location.Y = point.Y;
            Location.Z = point.Z;
            Location.Rotation = point.Rotation;
            Location.HeadRotation = point.HeadRotation;

            NeedsUpdate = true;
        }
    }
}
