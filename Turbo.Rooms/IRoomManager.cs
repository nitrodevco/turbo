using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public interface IRoomManager : IRoomContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public IRoomModel GetModel(int id);
        public IRoomModel GetModelByName(string name);
    }
}
