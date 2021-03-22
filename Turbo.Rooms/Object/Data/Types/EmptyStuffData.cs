using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data.Types
{
    public class EmptyStuffData : StuffDataBase
    {
        public string Data { get; set; }

        public override void WriteToPacket(IServerPacket packet)
        {
            packet.WriteInteger(Flags);

            base.WriteToPacket(packet);
        }

        public override string GetLegacyString()
        {
            return Data == null ? "" : Data;
        }
    }
}
