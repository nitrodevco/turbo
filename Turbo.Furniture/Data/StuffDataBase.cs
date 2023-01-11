using System;
using System.Text.Json.Serialization;
using Turbo.Core.Game.Furniture.Data;

namespace Turbo.Furniture.Data
{
    public class StuffDataBase : IStuffData
    {
        [JsonIgnore]
        public int Flags { get; set; }
        public int UniqueNumber { get; set; }
        public int UniqueSeries { get; set; }

        public StuffDataBase()
        {
        }

        public virtual string GetLegacyString()
        {
            return "";
        }

        public virtual void SetState(string state)
        {
            return;
        }

        public int GetState()
        {
            int state = int.Parse(GetLegacyString());

            return state;
        }

        public bool IsUnique()
        {
            return UniqueSeries > 0;
        }
    }
}
