using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Utils;

public interface IRollerData
{
    public IPoint Location { get; }
    public IPoint LocationNext { get; }

    public IRoomObjectFloor Roller { get; set; }
    public IDictionary<int, IRollerItemData<IRoomObjectAvatar>> Avatars { get; }
    public IDictionary<int, IRollerItemData<IRoomObjectFloor>> Furniture { get; }

    public void RemoveRoomObject(IRoomObject roomObject);
    public void CommitRoll();
    public void CompleteRoll();
}