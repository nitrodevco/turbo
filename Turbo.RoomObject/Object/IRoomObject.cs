using System;
using Turbo.RoomObject.Utils;

namespace Turbo.RoomObject.Object
{
    public interface IRoomObject : IDisposable
    {
        public int Id { get; }
        public string Type { get; }
        public IPoint Location { get; }
        public IPoint Direction { get; }

        public void SetLocation(IPoint point);
        public void SetDirection(IPoint point);
    }
}
