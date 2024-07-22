using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record SetRoomBackgroundColorDataMessage : IMessageEvent
{
    public int FurniId { get; init; }
    public int Hue { get; init; }
    public int Saturation { get; init; }
    public int Lightness { get; init; }
}