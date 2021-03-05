using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;

namespace Turbo.Packets.Parsers.Handshake
{
    public class VersionCheckParser : AbstractParser<VersionCheckMessage>
    {
        public override IMessageEvent Parse(IClientPacket packet)
        {
            return new VersionCheckMessage
            {
                ClientID = packet.PopInt(),
                ClientURL = packet.PopString(),
                ExternalVariablesURL = packet.PopString()
            };
        }
    }
}
