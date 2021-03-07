using System;

namespace Turbo.Rooms.Mapping
{
    public interface IRoomMap : IDisposable
    {
        public void GenerateMap();
    }
}
