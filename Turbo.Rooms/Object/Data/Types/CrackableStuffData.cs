using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data.Types
{
    public class CrackableStuffData : StuffDataBase
    {
        public string State { get; set; }
        public int Hits { get; set; }
        public int Target { get; set; }

        public override void WriteToPacket(IServerPacket packet)
        {
            packet.WriteInteger(Flags);
            packet.WriteString(State);
            packet.WriteInteger(Hits);
            packet.WriteInteger(Target);

            base.WriteToPacket(packet);
        }

        public override string GetLegacyString()
        {
            return State == null ? "" : State;
        }

        public override void SetState(string state)
        {
            State = state;
        }
    }
}
