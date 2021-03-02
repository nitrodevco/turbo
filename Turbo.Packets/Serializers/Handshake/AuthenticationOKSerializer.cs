using Turbo.Packets.Headers;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Serializers;

namespace Turbo.Packets.Composers.Handshake
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
