using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Utils
{
    public interface IRollerData
    {
        public IPoint Location { get; }
        public IPoint LocationNext { get; }

        public IRoomObject Roller { get; set; }
        public IDictionary<int, IRollerItemData> Users { get; }
        public IDictionary<int, IRollerItemData> Furniture { get; }

        public void RemoveRoomObject(IRoomObject roomObject);
        public void CommitRoll();
        public void CompleteRoll();
    }
}
