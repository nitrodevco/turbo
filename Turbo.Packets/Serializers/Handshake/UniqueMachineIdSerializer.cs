using Turbo.Packets.Headers;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Outgoing.Handshake;

namespace Turbo.Packets.Serializers.Handshake
{
    class UniqueMachineIdSerializer : AbstractSerializer<UniqueMachineIdMessage>
    {
        public UniqueMachineIdSerializer() : base(DefaultOutgoing.UniqueMachineID)
        {

        }

        protected override void Serialize(IServerPacket packet, UniqueMachineIdMessage message)
        {
            packet.WriteString(message.MachineID);
        }
    }
}
