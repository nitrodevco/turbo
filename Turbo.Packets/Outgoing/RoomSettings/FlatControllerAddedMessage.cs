using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings
{
    public record FlatControllerAddedMessage : IComposer
    {
        public int RoomId { get; init; }
        public int PlayerId { get; init; }
        public string PlayerName { get; init; }
    }
}