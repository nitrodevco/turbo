using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record OpenWelcomeGiftMessage : IMessageEvent
{
    public int FurniId { get; init; }
}