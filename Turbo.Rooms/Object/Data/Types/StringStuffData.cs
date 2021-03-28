using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data.Types
{
    public class StringStuffData : StuffDataBase
    {
        private static int _state = 0;

        public IList<string> Data { get; private set; }

        public override string GetLegacyString()
        {
            return GetValue(_state);
        }

        public override void SetState(string state)
        {
            Data[_state] = state;
        }

        public string GetValue(int index)
        {
            return Data[index];
        }
    }
}
