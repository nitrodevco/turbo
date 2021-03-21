using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Data;

namespace Turbo.Rooms.Object.Data
{
    public class StuffDataBase : IStuffData
    {
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

            if((uniqueNumber > 0) && (uniqueSeries > 0))
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
            int state = Int32.Parse(GetLegacyString());

            return state;
        }

        public bool IsUnique()
        {
            return UniqueSeries > 0;
        }
    }
}
