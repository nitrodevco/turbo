using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Inventory.Furni;

public record RequestRoomPropertySetMessage : IMessageEvent
{
    public int StripId { get; init; }
}