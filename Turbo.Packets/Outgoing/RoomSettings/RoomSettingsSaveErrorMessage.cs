using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public record RoomSettingsSaveErrorMessage : IComposer
{
    public int RoomId { get; init; }
    public RoomSettingsErrorType ErrorCode { get; init; }
    public string Info { get; init; }
}