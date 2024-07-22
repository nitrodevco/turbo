using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.RoomSettings;

public record GetFlatControllersMessage : IMessageEvent
{
    public int RoomId { get; init; }
}