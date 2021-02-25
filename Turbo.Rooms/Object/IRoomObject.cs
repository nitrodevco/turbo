using System;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object
{
    public interface IRoomObject
    {
        public int Id { get; }
        public string Type { get; }
        public void SetLocation(IPoint point);
        public void SetDirection(IPoint point);
    }
}
