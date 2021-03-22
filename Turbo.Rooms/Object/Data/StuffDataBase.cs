using System.Text.Json.Serialization;
using Turbo.Core.Game.Rooms.Object.Data;

namespace Turbo.Rooms.Object.Data
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

        public virtual bool InitializeFromFurnitureData(string data)
        {
            int uniqueNumber = 0;
            int uniqueSeries = 0;

            if ((uniqueNumber > 0) && (uniqueSeries > 0))
            {
                Flags += (int)StuffDataFlags.UniqueSet;
            }

            return true;
        }

        public virtual string GetLegacyString()
        {
            return "";
        }

        public virtual void SetState(string data)
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
