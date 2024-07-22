using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Chat;

public record ChatMessage : IMessageEvent
{
    public string Text { get; init; }
    public int StyleId { get; init; }
    public int ChatTrackingId { get; init; }
}