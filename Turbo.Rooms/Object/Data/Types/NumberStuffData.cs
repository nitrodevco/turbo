using System.Collections.Generic;

namespace Turbo.Rooms.Object.Data.Types
{
    public class NumberStuffData : StuffDataBase
    {
        private static int _state = 0;

        public IList<int> Data { get; private set; }

        public override string GetLegacyString()
        {
            return GetValue(_state).ToString();
        }

        public override void SetState(string state)
        {
            Data[_state] = int.Parse(state);
        }

        public int GetValue(int index)
        {
            return Data[index];
        }
    }
}
