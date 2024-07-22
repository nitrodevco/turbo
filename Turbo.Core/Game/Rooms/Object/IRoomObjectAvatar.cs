using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object;

public interface IRoomObjectAvatar : IRoomObject
{
    public IRoomObjectAvatarHolder RoomObjectHolder { get; }
    public IMovingAvatarLogic Logic { get; }
    public IPoint Location { get; }

    public int X { get; set; }
    public int Y { get; set; }
    public double Z { get; set; }
    public Rotation Rotation { get; set; }
    public Rotation HeadRotation { get; set; }

    public bool SetHolder(IRoomObjectAvatarHolder roomObjectHolder);
    public void SetLogic(IMovingAvatarLogic logic);
    public void SetLocation(IPoint point);
}