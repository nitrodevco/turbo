using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Chat;

public record WhisperMessage : IMessageEvent
{
    public string RecipientName { get; init; }
    public string Text { get; init; }
    public int StyleId { get; init; }
}