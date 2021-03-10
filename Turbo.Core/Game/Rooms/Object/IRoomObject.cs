using System;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObject : IDisposable
    {
        public int Id { get; }
        public string Type { get; }

        public IPoint Location { get; }
        public IPoint Direction { get; }

        public void SetLocation(IPoint point);
        public void SetDirection(IPoint point);
        public bool SetHolder(IRoomObjectHolder roomObjectHolder);
    }
}
