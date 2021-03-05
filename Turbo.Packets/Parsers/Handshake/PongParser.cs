using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;

namespace Turbo.Packets.Parsers.Handshake
{
    public class PongParser : AbstractParser<PongMessage>
    {
        public override IMessageEvent Parse(IClientPacket packet)
        {
            return new PongMessage
            {

            };
        }
    }
}
