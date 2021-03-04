using Turbo.Packets.Headers;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Outgoing.Handshake;

namespace Turbo.Packets.Serializers.Handshake
{
    public class PingSerializer : AbstractSerializer<PingMessage>
    {
        public PingSerializer() : base(DefaultOutgoing.Ping)
        {

        }

        protected override void Serialize(IServerPacket packet, PingMessage message)
        {
            
        }
    }
}
