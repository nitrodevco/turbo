using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data.Types
{
    public class StringStuffData : StuffDataBase
    {
        private static int _state = 0;

        public IList<string> Data { get; private set; }

        public override void WriteToPacket(IServerPacket packet)
        {
            packet.WriteInteger(Flags);
            packet.WriteInteger(Data.Count);

            foreach (string value in Data)
            {
                packet.WriteString(value);
            }

            base.WriteToPacket(packet);
        }

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
