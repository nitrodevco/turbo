using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object;

public class RoomObjectAvatar : RoomObject, IRoomObjectAvatar
{
    private IRoomObjectContainer<IRoomObjectAvatar> _roomObjectContainer;

    public RoomObjectAvatar(IRoom room, IRoomObjectContainer<IRoomObjectAvatar> roomObjectContainer, int id) :
        base(room, id)
    {
        _roomObjectContainer = roomObjectContainer;

        Location = new Point();
    }

    public IRoomObjectAvatarHolder RoomObjectHolder { get; protected set; }
    public IMovingAvatarLogic Logic { get; protected set; }
    public IPoint Location { get; }

    public virtual bool SetHolder(IRoomObjectAvatarHolder roomObjectHolder)
    {
        if (roomObjectHolder == null) return false;

        RoomObjectHolder = roomObjectHolder;

        return true;
    }

    public virtual void SetLogic(IMovingAvatarLogic logic)
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

    public virtual void SetLocation(IPoint point)
    {
        if (point == null) return;

        if (point.X == Location.X && point.Y == Location.Y && point.Z == Location.Z &&
            point.Rotation == Location.Rotation && point.HeadRotation == Location.HeadRotation) return;

        Location.X = point.X;
        Location.Y = point.Y;
        Location.Z = point.Z;
        Location.Rotation = point.Rotation;
        Location.HeadRotation = point.HeadRotation;

        NeedsUpdate = true;
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
        // implement saving for objects that need saving
    }
}