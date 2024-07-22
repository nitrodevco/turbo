using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.RoomSettings;

public record UpdateRoomFilterMessage : IMessageEvent
{
    public int RoomId { get; init; }
    public bool IsAddingWord { get; init; }
    public string Word { get; init; }
}