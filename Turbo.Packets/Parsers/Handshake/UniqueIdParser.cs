using System;
using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;

namespace Turbo.Packets.Parsers.Handshake
{
    public class UniqueIdParser : AbstractParser<UniqueIdMessage>
    {
        public override IMessageEvent Parse(IClientPacket packet)
        {
            return new UniqueIdMessage
            {
                MachineID = packet.PopString(),
                Fingerprint = packet.PopString(),
                FlashVersion = packet.PopString()
            };
        }
    }
}
