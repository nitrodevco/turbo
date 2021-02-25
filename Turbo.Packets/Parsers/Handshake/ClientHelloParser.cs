using System;
using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;

namespace Turbo.Packets.Parsers.Handshake
{
    public class ClientHelloParser : IParser<ClientHelloMessage>
    {
        ClientHelloMessage IParser<ClientHelloMessage>.Parse(ClientPacket packet)
        {
            return new ClientHelloMessage(packet.PopString(), packet.PopString(), packet.PopInt(), packet.PopInt());
        }
    }
}
