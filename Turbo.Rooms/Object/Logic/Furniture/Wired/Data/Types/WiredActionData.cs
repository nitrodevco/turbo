using System.Collections.Generic;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types
{
    public class WiredActionData : WiredDataBase
    {
        public int Delay { get; protected set; }
        public IList<int> Conflicts { get; protected set; }

        public WiredActionData() : base()
        {
            Delay = 0;
            Conflicts = new List<int>();
        }
    }
}
