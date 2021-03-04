using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;

namespace Turbo.Packets.Parsers.Handshake
{
    public class InfoRetrieveParser : AbstractParser<InfoRetrieveMessage>
    {
        public override IMessageEvent Parse(IClientPacket packet)
        {
            return new InfoRetrieveMessage { };
        }
    }
}
