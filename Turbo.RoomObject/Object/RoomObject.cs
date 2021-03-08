using System;
using Turbo.RoomObject.Utils;

namespace Turbo.RoomObject.Object
{
    public class RoomObject : IRoomObject
    {
        public int Id { get; private set; }
        public string Type { get; private set; }

        public IPoint Location { get; private set; }
        public IPoint Direction { get; private set; }

        public RoomObject(int id, string type)
        {
            Id = id;
            Type = type;

            Location = new Point();
            Direction = new Point();
        }

        public void Dispose()
        {

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
    }
}
