using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class ToggleStaffPickMessage : IMessageEvent
{
    public int RoomId { get; init; }
    public bool IsStaffPicked { get; init; }
}