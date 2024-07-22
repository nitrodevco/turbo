using System;
using System.Collections.Generic;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Rooms.Mapping;

public class RoomTile : IRoomTile
{
    private readonly object _avatarLock = new();
    private readonly object _furnitureLock = new();

    private double _height;
    private double _stackHelperHeight;

    public RoomTile(IPoint location, double height, RoomTileState state)
    {
        Location = location;
        DefaultHeight = height;

        RelativeHeight = DefaultSettings.TileHeightDefault;
        State = state;

        Avatars = [];
        Furniture = [];

        _height = DefaultHeight;

        ResetRelativeHeight();
    }

    public IPoint Location { get; }
    public double DefaultHeight { get; }

    public int RelativeHeight { get; private set; }
    public RoomTileState State { get; }
    public IRoomObjectFloor HighestObject { get; private set; }

    public List<IRoomObjectAvatar> Avatars { get; }
    public List<IRoomObjectFloor> Furniture { get; }

    public bool IsDoor { get; set; }
    public bool HasStackHelper { get; private set; }

    public void AddRoomObject(IRoomObject roomObject)
    {
        if (roomObject == null) return;

        if (roomObject is IRoomObjectAvatar avatarObject)
        {
            if (IsDoor) return;

            lock (_avatarLock)
            {
                if (!Avatars.Contains(avatarObject)) Avatars.Add(avatarObject);
            }

            return;
        }

        if (roomObject is IRoomObjectFloor floorObject)
            lock (_furnitureLock)
            {
                if (!Furniture.Contains(floorObject)) Furniture.Add(floorObject);

                ResetHighestObject();
            }
    }

    public void RemoveRoomObject(IRoomObject roomObject)
    {
        if (roomObject == null) return;

        if (roomObject is IRoomObjectAvatar avatarObject)
        {
            lock (_avatarLock)
            {
                Avatars.Remove(avatarObject);
            }

            return;
        }

        if (roomObject is IRoomObjectFloor floorObject)
            lock (_furnitureLock)
            {
                Furniture.Remove(floorObject);

                ResetHighestObject();
            }
    }

    public IRoomObjectFloor GetFurnitureAbove(IRoomObjectFloor floorObject)
    {
        if (floorObject == null) return null;

        var index = Furniture.IndexOf(floorObject);

        if (index >= 0 && index < Furniture.Count - 1) return Furniture[index + 1];

        return null;
    }

    public IRoomObjectFloor GetFurnitureUnderneath(IRoomObjectFloor floorObject)
    {
        if (floorObject == null) return null;

        var index = Furniture.IndexOf(floorObject);

        if (index > 0) return Furniture[index - 1];

        return null;
    }

    public void ResetTileHeight()
    {
        _height = DefaultHeight;

        if (HighestObject != null) _height = HighestObject.Logic.Height;

        ResetRelativeHeight();
    }

    public void ResetHighestObject()
    {
        _height = DefaultHeight;
        HighestObject = null;

        HasStackHelper = false;
        _stackHelperHeight = 0.0;

        if (Furniture.Count > 0)
        {
            Furniture.Sort((a, b) => a.Logic.Height.CompareTo(b.Logic.Height));

            foreach (var floorObject in Furniture)
            {
                var height = floorObject.Logic.Height;

                if (floorObject.Logic is FurnitureStackHelperLogic)
                {
                    HasStackHelper = true;
                    _stackHelperHeight = Math.Round(height, 3);

                    continue;
                }

                if (height < _height) continue;

                HighestObject = floorObject;
                _height = Math.Round(height, 3);
            }
        }

        ResetRelativeHeight();
    }

    public double GetWalkingHeight()
    {
        var height = _height;

        if (HighestObject != null)
            if (HighestObject.Logic.CanSit() || HighestObject.Logic.CanLay())
                height -= HighestObject.Logic.StackHeight;

        return Math.Round(height, 3);
    }

    public bool HasLogic(Type type)
    {
        foreach (var roomObject in Furniture)
            if (roomObject.Logic.GetType() == type)
                return true;

        return false;
    }

    public bool CanWalk(IRoomObjectAvatar avatar = null)
    {
        if (State == RoomTileState.Closed) return false;

        if (HighestObject != null)
        {
            if (!HighestObject.Logic.IsOpen(avatar)) return false;

            if (HasStackHelper && _stackHelperHeight >= HighestObject.Logic.Height) return false;

            if (!CanSit(avatar) && !CanLay(avatar))
            {
                var secondHighestObject = GetFurnitureUnderneath(HighestObject);

                if (secondHighestObject != null)
                    if ((secondHighestObject.Logic?.Height ?? 0) >= (HighestObject.Location?.Z ?? 0))
                        return false;
            }
        }

        if (HasStackHelper) return false;

        return true;
    }

    public bool CanSit(IRoomObjectAvatar avatar = null)
    {
        if (HighestObject != null && HighestObject.Logic.CanSit(avatar)) return true;

        return false;
    }

    public bool CanLay(IRoomObjectAvatar avatar = null)
    {
        if (HighestObject != null && HighestObject.Logic.CanLay(avatar)) return true;

        return false;
    }

    public bool CanStack()
    {
        if (HighestObject != null && !HighestObject.Logic.CanStack()) return false;

        return true;
    }

    public double Height
    {
        get
        {
            if (HasStackHelper) return _stackHelperHeight;

            return _height;
        }
    }

    private void ResetRelativeHeight()
    {
        RelativeHeight = DefaultSettings.TileHeightDefault;

        if (State == RoomTileState.Closed || !CanStack()) return;

        RelativeHeight = (int)Math.Ceiling((decimal)(HasStackHelper ? _stackHelperHeight : _height) *
                                           DefaultSettings.TileHeightMultiplier);
    }
}