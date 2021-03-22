using System;
using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data.Types
{
    public class NumberStuffData : StuffDataBase
    {
        private static int _state = 0;

        public IList<int> Data { get; private set; }

        public override void WriteToPacket(IServerPacket packet)
        {
            packet.WriteInteger(Flags);
            packet.WriteInteger(Data.Count);

            foreach (int value in Data)
            {
                packet.WriteInteger(value);
            }

            base.WriteToPacket(packet);
        }

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
