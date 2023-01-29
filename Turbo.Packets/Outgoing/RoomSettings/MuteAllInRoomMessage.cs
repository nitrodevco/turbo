using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings
{
    public class MuteAllInRoomMessage : IComposer
    {
        public bool Muted { get; init; }
    }
}