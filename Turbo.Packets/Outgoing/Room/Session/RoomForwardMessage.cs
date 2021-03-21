﻿using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session
{
    public record RoomForwardMessage : IComposer
    {
        public int RoomId { get; init; }
    }
}