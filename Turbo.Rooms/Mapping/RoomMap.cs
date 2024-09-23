using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.PathFinder;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping;

public class RoomMap : IRoomMap
{
    private readonly IDictionary<int, IDictionary<int, IRoomTile>> _map =
        new Dictionary<int, IDictionary<int, IRoomTile>>();

    private readonly IRoom _room;

    public RoomMap(IRoom room)
    {
        _room = room;

        PathFinder = new PathFinder.PathFinder(this);
    }

    public byte[,] Map { get; private set; }
    public IList<IRoomTile> Tiles { get; } = new List<IRoomTile>();
    public IPathFinder PathFinder { get; init; }

    public void Dispose()
    {
        _map.Clear();
        Tiles.Clear();
    }

    public void GenerateMap()
    {
        var roomModel = _room.RoomModel;

        if (roomModel == null) return;

        _map.Clear();
        Tiles.Clear();

        var totalX = roomModel.TotalX;
        var totalY = roomModel.TotalY;

        if (totalX == 0 || totalY == 0) return;

        Map = new byte[totalX, totalY];

        for (var y = 0; y < totalY; y++)
        for (var x = 0; x < totalX; x++)
            Map[x, y] = 1;

        for (var y = 0; y < totalY; y++)
        for (var x = 0; x < totalX; x++)
        {
            var height = roomModel.GetTileHeight(x, y);
            var state = roomModel.GetTileState(x, y);

            var roomTile = new RoomTile(new Point(x, y), height, state);

            if (_map.Count - 1 < x) _map.Add(x, new Dictionary<int, IRoomTile>());

            _map[x].Add(y, roomTile);

            Tiles.Add(roomTile);
        }

        var doorTile = GetTile(roomModel.DoorLocation);

        if (doorTile != null) doorTile.IsDoor = true;
    }

    public IRoomTile GetTile(IPoint point)
    {
        if (point == null || !_map.ContainsKey(point.X) || !_map[point.X].ContainsKey(point.Y)) return null;

        var roomTile = _map[point.X][point.Y];

        if (roomTile == null || roomTile.State == RoomTileState.Closed) return null;

        return roomTile;
    }

    public IRoomTile GetValidTile(IRoomObjectAvatar avatarObject, IPoint point, bool isGoal = true,
        bool blockingDisabled = false)
    {
        if (avatarObject == null || point == null) return null;

        var roomTile = GetTile(point);

        if (roomTile == null || roomTile.State == RoomTileState.Closed) return null;

        if (roomTile.IsDoor) return roomTile;

        if (roomTile.Avatars.Count > 0)
        {
            if (roomTile.Avatars.Contains(avatarObject)) return roomTile;

            if (blockingDisabled)
            {
                if (isGoal) return null;
            }
            else
            {
                return null;
            }
        }

        if (!roomTile.CanWalk(avatarObject)) return null;

        return roomTile;
    }

    public IRoomTile GetValidDiagonalTile(IRoomObjectAvatar avatarObject, IPoint point, bool blockingDisabled = false)
    {
        if (avatarObject == null || point == null) return null;

        var roomTile = GetTile(point);

        if (roomTile == null || roomTile.State == RoomTileState.Closed) return null;

        if (roomTile.IsDoor) return roomTile;

        if (roomTile.Avatars.Count > 0)
        {
            if (roomTile.Avatars.Contains(avatarObject)) return roomTile;

            if (!blockingDisabled) return null;
        }

        if (roomTile.CanSit(avatarObject) || roomTile.CanLay(avatarObject) ||
            !roomTile.CanWalk(avatarObject)) return null;

        return roomTile;
    }

    public IPoint GetValidPillowPoint(IRoomObjectAvatar avatarObject, IRoomObjectFloor floorObject,
        IPoint originalPoint)
    {
        var pillowPoints = AffectedPoints.GetPillowPoints(floorObject);

        if (pillowPoints == null || pillowPoints.Count == 0) return null;

        originalPoint = originalPoint.Clone();

        foreach (var point in pillowPoints)
        {
            if (floorObject.Location.Rotation == Rotation.North) originalPoint.Y = point.Y;
            else originalPoint.X = point.X;

            var roomTile = GetValidTile(avatarObject, originalPoint);

            if (roomTile != null) return roomTile.Location.Clone();
        }

        return null;
    }

    public IRoomTile GetHighestTileForRoomObject(IRoomObjectFloor floorObject)
    {
        var points = AffectedPoints.GetPoints(floorObject);

        IRoomTile highestTile = null;

        foreach (var point in points)
        {
            var roomTile = GetTile(point);

            if (roomTile == null) continue;

            if (highestTile == null)
            {
                highestTile = roomTile;

                continue;
            }

            if (highestTile == null) continue;

            var height = roomTile.Height;

            if (roomTile.HighestObject == floorObject) height -= roomTile.HighestObject.Logic.Height;

            if (height < highestTile.Height) continue;

            highestTile = roomTile;
        }

        return highestTile;
    }

    public void AddFloorObject(IRoomObjectFloor floorObject)
    {
        if (floorObject == null) return;

        List<IPoint> points = new();

        var affectedPoints = AffectedPoints.GetPoints(floorObject);

        if (affectedPoints.Count > 0)
            foreach (var affectedPoint in affectedPoints)
            {
                var roomTile = GetTile(affectedPoint);

                if (roomTile == null) continue;

                roomTile.AddRoomObject(floorObject);

                points.Add(affectedPoint);
            }

        if (!_room.IsInitialized) return;

        _room.SendComposer(new ObjectAddMessage
        {
            Object = floorObject,
            OwnerName = floorObject.RoomObjectHolder?.PlayerName ?? ""
        });

        UpdatePoints(true, points.ToArray());
    }

    public void AddWallObject(IRoomObjectWall wallObject)
    {
        if (wallObject == null) return;

        if (!_room.IsInitialized) return;

        _room.SendComposer(new ItemAddMessage
        {
            Object = wallObject,
            OwnerName = wallObject.RoomObjectHolder?.PlayerName ?? ""
        });
    }

    public void AddAvatarObject(IRoomObjectAvatar avatarObject)
    {
        if (avatarObject == null) return;

        var roomTile = GetTile(avatarObject.Location);

        roomTile?.AddRoomObject(avatarObject);

        avatarObject.Logic.InvokeCurrentLocation();

        avatarObject.NeedsUpdate = false;

        if (!_room.IsInitialized) return;

        var list = new List<IRoomObjectAvatar> { avatarObject };

        _room.SendComposer(new UsersMessage
        {
            RoomObjects = list
        });

        _room.SendComposer(new UserUpdateMessage
        {
            RoomObjects = list
        });
    }

    public void MoveFloorRoomObject(IRoomObjectFloor floorObject, IPoint oldLocation, bool sendUpdate = true)
    {
        List<IPoint> points = new();

        if (oldLocation != null)
        {
            var oldAffectedPoints = AffectedPoints.GetPoints(floorObject, oldLocation);

            if (oldAffectedPoints.Count > 0)
                foreach (var point in oldAffectedPoints)
                {
                    var roomTile = GetTile(point);

                    if (roomTile == null) continue;

                    roomTile.RemoveRoomObject(floorObject);

                    points.Add(point);
                }
        }

        var newAffectedPoints = AffectedPoints.GetPoints(floorObject);

        if (newAffectedPoints.Count > 0)
            foreach (var point in newAffectedPoints)
            {
                var roomTile = GetTile(point);

                if (roomTile == null) continue;

                roomTile.AddRoomObject(floorObject);

                points.Add(point);
            }

        if (!_room.IsInitialized) return;

        if (points.Count > 0) UpdatePoints(true, points.ToArray());

        if (sendUpdate)
            _room.SendComposer(new ObjectUpdateMessage
            {
                Object = floorObject
            });
    }

    public void MoveWallRoomObject(IRoomObjectWall wallObject, string oldLocation, bool sendUpdate = true)
    {
        if (!_room.IsInitialized) return;

        if (sendUpdate)
            _room.SendComposer(new ItemUpdateMessage
            {
                Object = wallObject
            });
    }

    public void RemoveFloorObject(IRoomObjectFloor floorObject, int pickerId = -1)
    {
        if (floorObject == null) return;

        List<IPoint> points = new();

        var affectedPoints = AffectedPoints.GetPoints(floorObject);

        if (affectedPoints.Count > 0)
            foreach (var affectedPoint in affectedPoints)
            {
                var roomTile = GetTile(affectedPoint);

                if (roomTile == null) continue;

                roomTile.RemoveRoomObject(floorObject);

                points.Add(affectedPoint);
            }

        if (!_room.IsInitialized) return;

        _room.SendComposer(new ObjectRemoveMessage
        {
            Id = floorObject.Id,
            IsExpired = false,
            PickerId = pickerId,
            Delay = 0
        });

        if (points.Count > 0) UpdatePoints(true, points.ToArray());
    }

    public void RemoveWallObject(IRoomObjectWall wallObject, int pickerId = -1)
    {
        if (wallObject == null) return;

        if (!_room.IsInitialized) return;

        _room.SendComposer(new ItemRemoveMessage
        {
            ItemId = wallObject.Id,
            PickerId = pickerId
        });
    }

    public void RemoveAvatarObject(IRoomObjectAvatar avatarObject)
    {
        if (avatarObject == null) return;

        avatarObject.Logic.StopWalking();

        var currentTile = avatarObject.Logic.GetCurrentTile();

        if (currentTile != null)
        {
            currentTile.HighestObject?.Logic?.OnLeave(avatarObject);
            currentTile.RemoveRoomObject(avatarObject);
        }

        avatarObject.Logic.GetNextTile()?.RemoveRoomObject(avatarObject);

        if (!_room.IsInitialized) return;

        _room.SendComposer(new UserRemoveMessage
        {
            Id = avatarObject.Id
        });
    }

    public void UpdatePoints(bool updateUsers = true, params IPoint[] points)
    {
        if (points.Length <= 0) return;

        List<IRoomTile> roomTiles = new();
        List<IRoomObjectAvatar> updatedAvatars = new();

        foreach (var point in points)
        {
            var roomTile = GetTile(point);

            if (roomTile == null || roomTiles.Contains(roomTile)) continue;

            if (updateUsers && roomTile.Avatars.Count > 0)
                foreach (var avatarObject in roomTile.Avatars)
                {
                    avatarObject.Logic.InvokeCurrentLocation();

                    updatedAvatars.Add(avatarObject);

                    avatarObject.NeedsUpdate = false;
                }

            roomTiles.Add(roomTile);

            if (roomTiles.Count == byte.MaxValue)
            {
                _room.SendComposer(new HeightMapUpdateMessage
                {
                    TilesToUpdate = roomTiles
                });

                roomTiles.Clear();
            }
        }

        if (roomTiles.Count > 0)
            _room.SendComposer(new HeightMapUpdateMessage
            {
                TilesToUpdate = roomTiles
            });

        if (updatedAvatars.Count > 0)
            _room.SendComposer(new UserUpdateMessage
            {
                RoomObjects = updatedAvatars
            });
    }

    public bool BlockingDisabled => _room.RoomDetails.BlockingDisabled;
}