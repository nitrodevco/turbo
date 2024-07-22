using System.Collections.Generic;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils;

public class RollerData : IRollerData
{
    private readonly IRoom _room;

    public RollerData(IRoom room, IPoint location, IPoint locationNext)
    {
        _room = room;

        Location = location;
        LocationNext = locationNext;

        Avatars = new Dictionary<int, IRollerItemData<IRoomObjectAvatar>>();
        Furniture = new Dictionary<int, IRollerItemData<IRoomObjectFloor>>();
    }

    public IPoint Location { get; }
    public IPoint LocationNext { get; }

    public IRoomObjectFloor Roller { get; set; }
    public IDictionary<int, IRollerItemData<IRoomObjectAvatar>> Avatars { get; }
    public IDictionary<int, IRollerItemData<IRoomObjectFloor>> Furniture { get; }

    public void RemoveRoomObject(IRoomObject roomObject)
    {
        if (roomObject is IRoomObjectAvatar avatarObject)
        {
            Avatars.Remove(roomObject.Id);

            return;
        }

        if (roomObject is IRoomObjectFloor floorObject) Furniture.Remove(roomObject.Id);
    }

    public void CommitRoll()
    {
        var currentTile = _room.RoomMap.GetTile(Location);
        var nextTile = _room.RoomMap.GetTile(LocationNext);

        if (currentTile == null || nextTile == null) return;

        if (Avatars.Count > 0)
            foreach (var rollerItemData in Avatars.Values)
            {
                var roomObject = rollerItemData.RoomObject;

                if (roomObject.Disposed)
                {
                    Avatars.Remove(roomObject.Id);

                    continue;
                }

                if (roomObject is IRoomObjectAvatar avatarObject)
                {
                    if (!avatarObject.Logic.IsRolling || !avatarObject.Location.Compare(Location) ||
                        avatarObject.Logic.RollerData != this)
                    {
                        Avatars.Remove(roomObject.Id);

                        avatarObject.Logic.RollerData = null;

                        continue;
                    }

                    avatarObject.Logic.GetCurrentTile()?.RemoveRoomObject(roomObject);

                    avatarObject.Location.X = LocationNext.X;
                    avatarObject.Location.Y = LocationNext.Y;
                    avatarObject.Location.Z = rollerItemData.HeightNext;

                    avatarObject.Logic.GetCurrentTile()?.AddRoomObject(roomObject);
                }
            }

        if (Furniture.Count > 0)
            foreach (var rollerItemData in Furniture.Values)
            {
                var roomObject = rollerItemData.RoomObject;

                if (roomObject.Disposed)
                {
                    Furniture.Remove(roomObject.Id);

                    continue;
                }

                if (roomObject is IRoomObjectFloor floorObject)
                {
                    if (!floorObject.Logic.IsRolling || !floorObject.Location.Compare(Location) ||
                        floorObject.Logic.RollerData != this)
                    {
                        Furniture.Remove(roomObject.Id);

                        floorObject.Logic.RollerData = null;

                        continue;
                    }

                    floorObject.Logic.GetCurrentTile()?.RemoveRoomObject(roomObject);

                    floorObject.Location.X = LocationNext.X;
                    floorObject.Location.Y = LocationNext.Y;
                    floorObject.Location.Z = rollerItemData.HeightNext;

                    if (floorObject.RoomObjectHolder is IRoomFloorFurniture furniture) furniture.Save();

                    floorObject.Logic.GetCurrentTile()?.AddRoomObject(roomObject);
                }
            }
    }

    public void CompleteRoll()
    {
        if (Avatars.Count > 0)
            foreach (var rollerItemData in Avatars.Values)
            {
                var avatarObject = rollerItemData.RoomObject;

                if (avatarObject.Disposed) continue;

                avatarObject.Logic.RollerData = null;
            }

        if (Furniture.Count > 0)
            foreach (var rollerItemData in Furniture.Values)
            {
                var floorObject = rollerItemData.RoomObject;

                if (floorObject.Disposed) continue;

                floorObject.Logic.RollerData = null;
            }
    }
}