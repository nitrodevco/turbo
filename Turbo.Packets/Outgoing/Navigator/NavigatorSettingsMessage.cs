using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record NavigatorSettingsMessage : IComposer
{
    public int HomeRoomId { get; init; }

    public int RoomIdToEnter { get; init; }
}