using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Chat;

public record RoomChatSettingsMessage : IComposer
{
    public RoomChatModeType ChatMode { get; init; }
    public RoomChatWeightType ChatWeight { get; init; }
    public RoomChatSpeedType ChatSpeed { get; init; }
    public int ChatDistance { get; init; }
    public RoomChatProtectionType ChatProtection { get; init; }
}