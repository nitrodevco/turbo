using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings
{
    public record NoSuchFlatMessage : IComposer
    {
        public int RoomId { get; init; }
    }
}