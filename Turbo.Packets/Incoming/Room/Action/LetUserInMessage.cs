using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action;

public record LetUserInMessage : IMessageEvent
{
    public string Username { get; init; }
    public bool CanEnter { get; init; }
}