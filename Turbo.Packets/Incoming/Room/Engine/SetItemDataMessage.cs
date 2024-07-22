using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record SetItemDataMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public string ColorHex { get; init; }
    public string Data { get; init; }
}