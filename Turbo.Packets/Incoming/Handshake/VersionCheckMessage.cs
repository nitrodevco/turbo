using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Incoming.Handshake
{
    public record VersionCheckMessage
    {
        public int ClientID { get; init; }
        public string ClientURL { get; init; }
        public string ExternalVariablesURL { get; init; }
    }
}
