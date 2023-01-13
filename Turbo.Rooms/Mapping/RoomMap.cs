using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public class RoomMap : IRoomMap
    {
        private readonly IRoom _room;
        private readonly IDictionary<int, IDictionary<int, IRoomTile>> _map;

        public byte[,] Map { get; private set; }
        public IList<IRoomTile> Tiles { get; init; }
        public IPathFinder PathFinder { get; init; }

        public RoomMap(IRoom room)
        {
            _room = room;
            _map = new Dictionary<int, IDictionary<int, IRoomTile>>();

            Tiles = new List<IRoomTile>();
            PathFinder = new PathFinder.PathFinder(this);
        }

        public void Dispose()
        {
            _map.Clear();
            Tiles.Clear();
        }

        public void GenerateMap()
        {
            IRoomModel roomModel = _room.RoomModel;

            if (roomModel == null) return;

            _map.Clear();
            Tiles.Clear();

            int totalX = roomModel.TotalX;
            int totalY = roomModel.TotalY;

            if ((totalX == 0) || (totalY == 0)) return;

            Map = new byte[totalX, totalY];

            for (int y = 0; y < totalY; y++)
                for (int x = 0; x < totalX; x++)
                    Map[x, y] = 1;

            for (int y = 0; y < totalY; y++)
            {
                for (int x = 0; x < totalX; x++)
                {
                    int height = roomModel.GetTileHeight(x, y);
                    var state = roomModel.GetTileState(x, y);

                    var roomTile = new RoomTile(new Point(x, y), height, state);

                    if (_map.Count - 1 < x) _map.Add(x, new Dictionary<int, IRoomTile>());

                    _map[x].Add(y, roomTile);

                    Tiles.Add(roomTile);
                }
            }

            var doorTile = GetTile(roomModel.DoorLocation);

            if (doorTile != null) doorTile.IsDoor = true;
        }

        public IRoomTile GetTile(IPoint point)
        {
            if ((point == null) || !_map.ContainsKey(point.X) || !_map[point.X].ContainsKey(point.Y)) return null;

            var roomTile = _map[point.X][point.Y];

            if (roomTile.State == RoomTileState.Closed) return null;

            return roomTile;
        }

        public IRoomTile GetValidTile(IRoomObjectAvatar avatarObject, IPoint point, bool isGoal = true, bool blockingDisabled = false)
        {
            if ((avatarObject == null) || (point == null)) return null;

            var roomTile = GetTile(point);

            if (roomTile == null) return null;

            if (roomTile.IsDoor) return roomTile;

            if (roomTile.Avatars.Count > 0)
            {
                foreach (var tileAvatarObject in roomTile.Avatars.Values)
                {
                    if (tileAvatarObject == avatarObject) return roomTile;
                }

                if (blockingDisabled)
                {
                    if (isGoal) return null;
                }
                else return null;
            }

            if (!roomTile.CanWalk(avatarObject)) return null;

            return roomTile;
        }

        public IRoomTile GetValidDiagonalTile(IRoomObjectAvatar avatarObject, IPoint point, bool blockingDisabled = false)
        {
            if ((avatarObject == null) || (point == null)) return null;

            var roomTile = GetTile(point);

            if (roomTile == null) return null;

            if (roomTile.IsDoor) return roomTile;

            if (roomTile.Avatars.Count > 0)
            {
                foreach (var tileAvatarObject in roomTile.Avatars.Values)
                {
                    if (tileAvatarObject == avatarObject) return roomTile;
                }

                if (!blockingDisabled) return null;
            }

            if (!roomTile.CanWalk(avatarObject) || roomTile.CanSit(avatarObject) || roomTile.CanLay(avatarObject)) return null;

            return roomTile;
        }

        public IPoint GetValidPillowPoint(IRoomObjectAvatar avatarObject, IRoomObjectFloor floorObject, IPoint originalPoint)
        {
            var pillowPoints = AffectedPoints.GetPillowPoints(floorObject);

            originalPoint = originalPoint.Clone();

            if ((pillowPoints == null) || (pillowPoints.Count == 0)) return null;

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

            foreach (IPoint point in points)
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

                if (roomTile.HighestObject == floorObject)
                {
                    height -= roomTile.HighestObject.Logic.Height;
                }

                if (height < highestTile.Height) continue;

                highestTile = roomTile;
            }

            return highestTile;
        }

        public void AddRoomObjects(params IRoomObject[] roomObjects)
        {
            if (roomObjects.Length <= 0) return;

            List<IRoomObjectFloor> floorObjects = new();
            List<IRoomObjectWall> wallObjects = new();
            List<IRoomObjectAvatar> avatarObjects = new();
            List<IPoint> points = new();

            foreach (IRoomObject roomObject in roomObjects)
            {
                if (roomObject is IRoomObjectFloor floorObject)
                {
                    var affectedPoints = AffectedPoints.GetPoints(floorObject);

                    if (affectedPoints.Count > 0)
                    {
                        foreach (var affectedPoint in affectedPoints)
                        {
                            var roomTile = GetTile(affectedPoint);

                            if (roomTile != null)
                            {
                                roomTile.AddRoomObject(floorObject);

                                points.Add(affectedPoint);
                            }
                        }
                    }

                    floorObjects.Add(floorObject);

                    continue;
                }

                if (roomObject is IRoomObjectWall wallObject)
                {
                    wallObjects.Add(wallObject);

                    continue;
                }

                if (roomObject is IRoomObjectAvatar avatarObject)
                {
                    var roomTile = GetTile(avatarObject.Location);

                    if (roomTile != null)
                    {
                        roomTile.AddRoomObject(roomObject);
                    }

                    avatarObject.Logic.InvokeCurrentLocation();

                    roomObject.NeedsUpdate = false;

                    avatarObjects.Add(avatarObject);

                    continue;
                }
            }

            if (!_room.IsInitialized) return;

            if (floorObjects.Count > 0)
            {
                if (floorObjects.Count == 1)
                {
                    _room.SendComposer(new ObjectAddMessage
                    {
                        Object = floorObjects[0]
                    });
                }
                else
                {
                    _room.SendComposer(new ObjectsMessage
                    {
                        Objects = floorObjects
                    });
                }

                UpdatePoints(true, points.ToArray());
            }

            if (wallObjects.Count > 0)
            {
                if (wallObjects.Count == 1)
                {
                    _room.SendComposer(new ItemAddMessage
                    {
                        Object = wallObjects[0]
                    });
                }
                else
                {
                    _room.SendComposer(new ItemsMessage
                    {
                        Objects = wallObjects
                    });
                }
            }

            if (avatarObjects.Count > 0)
            {
                _room.SendComposer(new UsersMessage
                {
                    RoomObjects = avatarObjects
                });

                _room.SendComposer(new UserUpdateMessage
                {
                    RoomObjects = avatarObjects
                });
            }
        }

        public void MoveFloorRoomObject(IRoomObjectFloor floorObject, IPoint oldLocation, bool sendUpdate = true)
        {
            List<IPoint> points = new();

            if (oldLocation != null)
            {
                var oldAffectedPoints = AffectedPoints.GetPoints(floorObject, oldLocation);

                if (oldAffectedPoints.Count > 0)
                {
                    foreach (var point in oldAffectedPoints)
                    {
                        var roomTile = GetTile(point);

                        if (roomTile == null) continue;

                        roomTile.RemoveRoomObject(floorObject);

                        points.Add(point);
                    }
                }
            }

            var newAffectedPoints = AffectedPoints.GetPoints(floorObject);

            if (newAffectedPoints.Count > 0)
            {
                foreach (var point in newAffectedPoints)
                {
                    var roomTile = GetTile(point);

                    if (roomTile == null) continue;

                    roomTile.AddRoomObject(floorObject);

                    points.Add(point);
                }
            }

            if (!_room.IsInitialized) return;

            UpdatePoints(true, points.ToArray());

            if (sendUpdate) _room.SendComposer(new ObjectUpdateMessage
            {
                Object = floorObject
            });
        }

        public void MoveWallRoomObject(IRoomObjectWall wallObject, string oldLocation, bool sendUpdate = true)
        {
            if (!_room.IsInitialized) return;

            if (sendUpdate) _room.SendComposer(new ItemUpdateMessage
            {
                Object = wallObject
            });
        }

        public void RemoveRoomObjects(int pickerId, params IRoomObject[] roomObjects)
        {
            if (roomObjects.Length <= 0) return;

            List<IPoint> points = new();

            foreach (var roomObject in roomObjects)
            {
                if (roomObject is IRoomObjectFloor floorObject)
                {
                    var affectedPoints = AffectedPoints.GetPoints(floorObject);

                    if (affectedPoints.Count > 0)
                    {
                        foreach (var affectedPoint in affectedPoints)
                        {
                            var roomTile = GetTile(affectedPoint);

                            if (roomTile != null)
                            {
                                roomTile.RemoveRoomObject(roomObject);

                                points.Add(affectedPoint);
                            }
                        }
                    }

                    if (_room.IsInitialized)
                    {
                        _room.SendComposer(new ObjectRemoveMessage
                        {
                            Id = floorObject.Id,
                            IsExpired = false,
                            PickerId = pickerId,
                            Delay = 0
                        });
                    }

                    continue;
                }

                if (roomObject is IRoomObjectWall wallObject)
                {
                    if (_room.IsInitialized)
                    {
                        _room.SendComposer(new ItemRemoveMessage
                        {
                            ItemId = wallObject.Id,
                            PickerId = pickerId
                        });
                    }

                    continue;
                }

                if (roomObject is IRoomObjectAvatar avatarObject)
                {
                    avatarObject.Logic.StopWalking();

                    avatarObject.Logic.GetCurrentTile()?.RemoveRoomObject(roomObject);
                    avatarObject.Logic.GetNextTile()?.RemoveRoomObject(roomObject);

                    if (_room.IsInitialized)
                    {
                        _room.SendComposer(new UserRemoveMessage
                        {
                            Id = roomObject.Id
                        });
                    }
                }
            }

            if (_room.IsInitialized && (points.Count > 0))
            {
                UpdatePoints(true, points.ToArray());
            }
        }

        public void UpdatePoints(bool updateUsers = true, params IPoint[] points)
        {
            if (points.Length <= 0) return;

            List<IRoomTile> roomTiles = new();
            List<IRoomObjectAvatar> updatedAvatars = new();

            foreach (var point in points)
            {
                var roomTile = GetTile(point);

                if ((roomTile == null) || roomTiles.Contains(roomTile)) continue;

                if (updateUsers && roomTile.Avatars.Count > 0)
                {
                    foreach (var avatarObject in roomTile.Avatars.Values)
                    {
                        avatarObject.Logic.InvokeCurrentLocation();

                        updatedAvatars.Add(avatarObject);

                        avatarObject.NeedsUpdate = false;
                    }
                }

                roomTiles.Add(roomTile);

                // check roomTiles count, if count equals max byte length, send height packet and clear the list
            }

            if (roomTiles.Count > 0)
            {
                _room.SendComposer(new HeightMapUpdateMessage
                {
                    TilesToUpdate = roomTiles
                });
            }

            if (updatedAvatars.Count > 0)
            {
                _room.SendComposer(new UserUpdateMessage
                {
                    RoomObjects = updatedAvatars
                });
            }
        }

        public bool BlockingDisabled
        {
            get => _room.RoomDetails.BlockingDisabled;
        }
    }
}
