using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record SlideObjectBundleMessage : IComposer
{
    public IPoint OldPos { get; init; }
    public IPoint NewPos { get; init; }
    public IRollerItemData<IRoomObjectAvatar> Avatar { get; init; }
    public IList<IRollerItemData<IRoomObjectFloor>> Furniture { get; init; }
    public int RollerItemId { get; init; } = 0;
    public SlideObjectMoveType MoveType { get; init; } = SlideObjectMoveType.Slide;
}