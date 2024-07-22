using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Avatar;

public record ChangeMottoMessage : IMessageEvent
{
    public string Motto { get; init; }
}