using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data.Types
{
    public class LegacyStuffData : StuffDataBase
    {
        public string Data { get; set; }

        public override void WriteToPacket(IServerPacket packet)
        {
            packet.WriteInteger(Flags);
            packet.WriteString(Data);

            base.WriteToPacket(packet);
        }

        public override string GetLegacyString()
        {
            return Data == null ? "" : Data;
        }

        public override void SetState(string state)
        {
            Data = state;
        }
    }
}
