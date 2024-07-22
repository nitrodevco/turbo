using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record SetClothingChangeDataMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public string Gender { get; init; }
    public string Clothes { get; init; }
}