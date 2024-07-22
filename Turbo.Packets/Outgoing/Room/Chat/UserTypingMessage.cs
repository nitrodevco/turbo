using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Chat;

public record UserTypingMessage : IComposer
{
    public int ObjectId { get; init; }
    public bool IsTyping { get; init; }
}