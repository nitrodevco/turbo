﻿namespace Turbo.Packets.Incoming.Room.Engine
{
    public record RemoveBotFromFlatMessage : IMessageEvent
    {
        public int BotId { get; init; }
    }
}
