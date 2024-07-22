using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record PlaceBotMessage : IMessageEvent
{
    public int BotId { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
}