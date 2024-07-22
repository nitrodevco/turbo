using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public record ShowEnforceRoomCategoryDialogMessage : IComposer
{
    public int SelectionType { get; init; }
}