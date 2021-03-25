using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectUserHolder : IRoomObjectHolder
    {
        public string Name { get; }
        public string Motto { get; }
        public string Figure { get; }
        public string Gender { get; }
    }
}
