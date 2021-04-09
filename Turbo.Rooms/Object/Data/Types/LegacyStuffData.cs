using System;

namespace Turbo.Rooms.Object.Data.Types
{
    public class LegacyStuffData : StuffDataBase
    {
        public string Data { get; set; }

        public LegacyStuffData()
        {
            if((Data == null) || Data.Equals("")) Data = "0";
        }

        public override string GetLegacyString()
        {
            return Data;
        }

        public override void SetState(string state)
        {
            Data = state;
        }
    }
}
