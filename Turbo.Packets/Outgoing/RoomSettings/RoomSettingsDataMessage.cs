using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings
{
    public class RoomSettingsDataMessage : IComposer
    {
        public IRoomDetails RoomDetails { get; init; }
    }
}