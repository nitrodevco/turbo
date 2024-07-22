using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public record RoomSettingsSavedMessage : IComposer
{
    public int RoomId { get; init; }
}