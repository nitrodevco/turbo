using Turbo.Core.Game.Rooms;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public class RoomSettingsDataMessage : IComposer
{
    public IRoomDetails RoomDetails { get; init; }
}