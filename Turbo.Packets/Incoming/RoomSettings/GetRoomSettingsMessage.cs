using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.RoomSettings;

public record GetRoomSettingsMessage : IMessageEvent
{
    public int RoomId { get; init; }
}