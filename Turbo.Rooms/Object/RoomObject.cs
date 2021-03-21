using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Messages;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object
{
    public class RoomObject : IRoomObject
    {
        public IRoom Room { get; private set; }
        public IRoomObjectHolder RoomObjectHolder { get; private set; }

        private IRoomObjectContainer _roomObjectContainer;

        public int Id { get; private set; }
        public string Type { get; private set; }

        public IPoint Location { get; private set; }

        public IRoomObjectLogic Logic { get; private set; }

        public bool NeedsUpdate { get; set; }

        private bool _isDisposing { get; set; }

        public RoomObject(IRoom room, IRoomObjectContainer roomObjectContainer, int id, string type)
        {
            Room = room;

            _roomObjectContainer = roomObjectContainer;

            Id = id;
            Type = type;

            Location = new Point();
        }

        public void Dispose()
        {
            if (Disposed || _isDisposing) return;

            _isDisposing = true;

            if (_roomObjectContainer != null) _roomObjectContainer.RemoveRoomObject(Id);

            SetLogic(null);

            if (RoomObjectHolder != null)
            {
                RoomObjectHolder.ClearRoomObject();

                RoomObjectHolder = null;
            }

            Id = -1;
            Room = null;
            NeedsUpdate = false;
            _roomObjectContainer = null;

            _isDisposing = false;
        }

        public void SetLocation(IPoint point)
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

        public bool SetHolder(IRoomObjectHolder roomObjectHolder)
        {
            if (roomObjectHolder == null) return false;

            Type = roomObjectHolder.Type;

            RoomObjectHolder = roomObjectHolder;

            return true;
        }

        public void SetLogic(IRoomObjectLogic logic)
        {
            if (logic == Logic) return;

            IRoomObjectLogic currentLogic = Logic;

            if(currentLogic != null)
            {
                Logic = null;

                currentLogic.SetRoomObject(null);
            }

            Logic = logic;

            if(Logic != null)
            {
                Logic.SetRoomObject(this);
            }
        }

        public bool Disposed => (_roomObjectContainer == null);
    }
}
