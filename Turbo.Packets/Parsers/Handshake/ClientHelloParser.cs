using System;
using System.Threading.Tasks;
using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Sessions;

namespace Turbo.Packets.Parsers.Handshake
{
    public class ClientHelloParser : AbstractParser<ClientHelloMessage>
    {
        public override IMessageEvent Parse(IClientPacket packet)
        {
            return new ClientHelloMessage
            {
                Production = packet.PopString(),
                Platform = packet.PopString(),
                ClientPlatform = packet.PopInt(),
                DeviceCategory = packet.PopInt()
            };
        }
    }
}
