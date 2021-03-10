﻿using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object
{
    public class RoomObject : IRoomObject
    {
        private readonly IRoomObjectContainer _roomObjectContainer;
        public IRoomObjectHolder RoomObjectHolder { get; private set; }

        public int Id { get; private set; }
        public string Type { get; private set; }

        public IPoint Location { get; private set; }
        public IPoint Direction { get; private set; }

        private bool _isDisposing { get; set; }

        public RoomObject(IRoomObjectContainer roomObjectContainer, int id, string type)
        {
            _roomObjectContainer = roomObjectContainer;

            Id = id;
            Type = type;

            Location = new Point();
            Direction = new Point();
        }

        public void Dispose()
        {
            if (_isDisposing) return;

            _isDisposing = true;

            if (_roomObjectContainer != null) _roomObjectContainer.RemoveRoomObject(Id);
        }

        public void SetLocation(IPoint point)
        {
            if (point == null) return;

            if ((point.X == Location.X) && (point.Y == Location.Y) && (point.Z == Location.Z)) return;

            Location.X = point.X;
            Location.Y = point.Y;
            Location.Z = point.Z;
        }

        public void SetDirection(IPoint point)
        {
            if (point == null) return;

            if ((point.X == Direction.X) && (point.Y == Direction.Y) && (point.Z == Direction.Z)) return;

            Direction.X = point.X;
            Direction.Y = point.Y;
            Direction.Z = point.Z;
        }

        public bool SetHolder(IRoomObjectHolder roomObjectHolder)
        {
            if (roomObjectHolder == null) return false;

            Type = roomObjectHolder.Type;

            RoomObjectHolder = roomObjectHolder;

            return true;
        }
    }
}
