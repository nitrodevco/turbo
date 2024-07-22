using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object;

public class RoomObjectFloor : RoomObject, IRoomObjectFloor
{
    private IRoomObjectContainer<IRoomObjectFloor> _roomObjectContainer;

    public RoomObjectFloor(IRoom room, IRoomObjectContainer<IRoomObjectFloor> roomObjectContainer, int id) : base(room,
        id)
    {
        Location = new Point();

        _roomObjectContainer = roomObjectContainer;
    }

    public IRoomObjectFloorHolder RoomObjectHolder { get; protected set; }
    public IFurnitureFloorLogic Logic { get; protected set; }
    public IPoint Location { get; }

    public virtual bool SetHolder(IRoomObjectFloorHolder roomObjectHolder)
    {
        if (roomObjectHolder == null) return false;

        RoomObjectHolder = roomObjectHolder;

        return true;
    }

    public virtual void SetLogic(IFurnitureFloorLogic logic)
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

    public virtual void SetLocation(IPoint point, bool save = true, bool update = true)
    {
        if (point == null) return;

        if (point.X == Location.X && point.Y == Location.Y && point.Z == Location.Z &&
            point.Rotation == Location.Rotation && point.HeadRotation == Location.HeadRotation) return;

        Location.X = point.X;
        Location.Y = point.Y;
        Location.Z = point.Z;
        Location.Rotation = point.Rotation;
        Location.HeadRotation = point.HeadRotation;

        if (save) Save();

        if (update) NeedsUpdate = true;
    }

    public int X
    {
        get => Location.X;
        set
        {
            Location.X = value;

            Save();
        }
    }

    public int Y
    {
        get => Location.Y;
        set
        {
            Location.Y = value;

            Save();
        }
    }

    public double Z
    {
        get => Location.Z;
        set
        {
            Location.Z = value;

            Save();
        }
    }

    public Rotation Rotation
    {
        get => Location.Rotation;
        set
        {
            Location.Rotation = value;

            Save();
        }
    }

    public Rotation HeadRotation
    {
        get => Location.HeadRotation;
        set
        {
            Location.HeadRotation = value;

            Save();
        }
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

    private void Save()
    {
        if (RoomObjectHolder is IRoomFloorFurniture floorFurniture) floorFurniture.Save();
    }
}