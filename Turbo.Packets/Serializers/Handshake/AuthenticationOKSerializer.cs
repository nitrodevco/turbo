using Turbo.Packets.Headers;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Outgoing.Handshake;

namespace Turbo.Packets.Serializers.Handshake
{
    public class AuthenticationOKSerializer : AbstractSerializer<AuthenticationOKMessage>
    {
        public AuthenticationOKSerializer() : base(DefaultOutgoing.AuthenticationOK)
        {
            
        }

        protected override void Serialize(IServerPacket packet, AuthenticationOKMessage message)
        {
            
        }
    }
}
