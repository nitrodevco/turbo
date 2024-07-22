using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record RoomReadyMessage : IComposer
{
    public string RoomType { get; init; }
    public int RoomId { get; init; }
}