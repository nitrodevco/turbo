using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public class MuteAllInRoomMessage : IComposer
{
    public bool Muted { get; init; }
}