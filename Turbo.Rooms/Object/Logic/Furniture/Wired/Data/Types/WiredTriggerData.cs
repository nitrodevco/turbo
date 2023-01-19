using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using System.Text.Json.Serialization;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types
{
    public class WiredTriggerData : WiredDataBase, IWiredTriggerData
    {
        [JsonIgnore]
        public IList<int> Conflicts { get; protected set; }

        public WiredTriggerData() : base()
        {
            Conflicts = new List<int>();
        }
    }
}
