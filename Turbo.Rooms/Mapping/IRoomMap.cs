using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Rooms.Mapping
{
    public interface IRoomMap
    {
        public void Dispose();
        public void GenerateMap();
    }
}
