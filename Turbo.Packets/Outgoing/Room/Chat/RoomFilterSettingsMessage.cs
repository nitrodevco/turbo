using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record RoomFilterSettingsMessage : IComposer
    {
        public IList<string> BadWords { get; init; }
    }
}
