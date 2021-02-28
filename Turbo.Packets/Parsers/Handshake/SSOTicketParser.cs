using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;

namespace Turbo.Packets.Parsers.Handshake
{
    class SSOTicketParser : AbstractParser<SSOTicketMessage>
    {
        public override IMessageEvent Parse(IClientPacket packet)
        {
            return new SSOTicketMessage
            {
                SSO = packet.PopString()
            };
        }
    }
}
