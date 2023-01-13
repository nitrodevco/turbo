using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object
{
    public class RoomObjectAvatar : RoomObject, IRoomObjectAvatar
    {
        public IRoomObjectAvatarHolder RoomObjectHolder { get; protected set; }
        public IMovingAvatarLogic Logic { get; protected set; }
        public IPoint Location { get; private set; }

        private IRoomObjectContainer<IRoomObjectAvatar> _roomObjectContainer;

        public RoomObjectAvatar(IRoom room, IRoomObjectContainer<IRoomObjectAvatar> roomObjectContainer, int id) : base(room, id)
        {
            _roomObjectContainer = roomObjectContainer;

            Location = new Point();
        }

        protected override void OnDispose()
        {
            if (_roomObjectContainer != null) _roomObjectContainer.RemoveRoomObject(Id);

            if (RoomObjectHolder != null)
            {
                RoomObjectHolder.ClearRoomObject();

                RoomObjectHolder = null;
            }

            SetLogic(null);

            _roomObjectContainer = null;
        }

        public virtual bool SetHolder(IRoomObjectAvatarHolder roomObjectHolder)
        {
            if (roomObjectHolder == null) return false;

            RoomObjectHolder = roomObjectHolder;

            return true;
        }

        public virtual void SetLogic(IMovingAvatarLogic logic)
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