using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Object;

public class RoomObjectWall : RoomObject, IRoomObjectWall
{
    private IRoomObjectContainer<IRoomObjectWall> _roomObjectContainer;

    public RoomObjectWall(IRoom room, IRoomObjectContainer<IRoomObjectWall> roomObjectContainer, int id) : base(room,
        id)
    {
        WallLocation = "";

        _roomObjectContainer = roomObjectContainer;
    }

    public IRoomObjectWallHolder RoomObjectHolder { get; protected set; }
    public IFurnitureWallLogic Logic { get; protected set; }
    public string WallLocation { get; private set; }

    public virtual bool SetHolder(IRoomObjectWallHolder roomObjectHolder)
    {
        if (roomObjectHolder == null) return false;

        RoomObjectHolder = roomObjectHolder;

        return true;
    }

    public virtual void SetLogic(IFurnitureWallLogic logic)
    {
        if (logic == Logic) return;

        var currentLogic = Logic;

        if (currentLogic != null)
        {
            Logic = null;

            currentLogic.SetRoomObject(null);
        }

        Logic = logic;

        if (Logic != null) Logic.SetRoomObject(this);
    }

    public virtual void SetLocation(string location)
    {
        if (location == null || location.Length == 0) return;

        // needs validation

        WallLocation = location;

        NeedsUpdate = true;
    }

    protected override void OnDispose()
    {
        if (_roomObjectContainer != null) _roomObjectContainer.RemoveRoomObject(Id);

        if (RoomObjectHolder != null)
        {
            RoomObjectHolder.ClearRoomObject();

            RoomObjectHolder = null;
        }

        SetLogic(null);

        _roomObjectContainer = null;
    }
}