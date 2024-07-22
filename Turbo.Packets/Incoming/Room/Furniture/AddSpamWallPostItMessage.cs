using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record AddSpamWallPostItMessage : IMessageEvent
{
    public int ItemId { get; init; }
    public string Location { get; init; }
    public string ColorHex { get; init; }
    public string Message { get; init; }
}