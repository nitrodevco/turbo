using System.Collections.Generic;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types
{
    public class WiredTriggerData : WiredDataBase
    {
        public IList<int> Conflicts { get; protected set; }

        public WiredTriggerData() : base()
        {
            Conflicts = new List<int>();
        }
    }
}
