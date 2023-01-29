using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings
{
    public record FlatControllersMessage : IComposer
    {
        public int RoomId { get; init; }
        public IDictionary<int, string> Players { get; init; }
    }
}