using System;

namespace Turbo.Core.Game.Rooms.Mapping
{
    public interface IRoomMap : IDisposable
    {
        public void GenerateMap();
    }
}
