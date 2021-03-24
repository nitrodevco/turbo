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

        public IList<IRoomTile> Tiles { get; init; }
        public IPathFinder PathFinder { get; init; }

        public RoomMap(IRoom room)
        {
            _room = room;

            _map = new Dictionary<int, IDictionary<int, IRoomTile>>();
            Tiles = new List<IRoomTile>();

            PathFinder = new PathFinder(this);
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

            for (int y = 0; y < totalY; y++)
            {
                for (int x = 0; x < totalX; x++)
                {
                    int height = roomModel.GetTileHeight(x, y);
                    RoomTileState state = roomModel.GetTileState(x, y);

                    IRoomTile roomTile = new RoomTile(new Point(x, y), height, state);

                    if (_map.Count - 1 < x) _map.Add(x, new Dictionary<int, IRoomTile>());

                    _map[x].Add(y, roomTile);

                    Tiles.Add(roomTile);
                }
            }

            IRoomTile doorTile = GetTile(roomModel.DoorLocation);

            if (doorTile != null) doorTile.IsDoor = true;
        }

        public IRoomTile GetTile(IPoint point)
        {
            if ((point == null) || !_map.ContainsKey(point.X) || !_map[point.X].ContainsKey(point.Y)) return null;

            IRoomTile roomTile = _map[point.X][point.Y];

            if (roomTile.State == RoomTileState.Closed) return null;

            return roomTile;
        }

        public IRoomTile GetValidTile(IRoomObject roomObject, IPoint point, bool isGoal = true)
        {
            if ((roomObject == null) || (point == null)) return null;

            IRoomTile roomTile = GetTile(point);

            if (roomTile == null) return null;

            if (roomTile.IsDoor) return roomTile;

            if (roomTile.Users.Count > 0)
            {
                foreach (IRoomObject tileRoomObject in roomTile.Users.Values)
                {
                    if (tileRoomObject == roomObject) return roomTile;
                }

                if (_room.RoomDetails.AllowWalkThrough)
                {
                    if (isGoal) return null;
                }
                else return null;
            }

            if (!roomTile.CanWalk()) return null;

            return roomTile;
        }

        public IRoomTile GetValidDiagonalTile(IRoomObject roomObject, IPoint point)
        {
            if ((roomObject == null) || (point == null)) return null;

            IRoomTile roomTile = GetTile(point);

            if (roomTile == null) return null;

            if (roomTile.IsDoor) return roomTile;

            if (roomTile.Users.Count > 0)
            {
                foreach (IRoomObject tileRoomObject in roomTile.Users.Values)
                {
                    if (tileRoomObject == roomObject) return roomTile;
                }

                if (!_room.RoomDetails.AllowWalkThrough) return null;
            }

            if (!roomTile.CanWalk() || roomTile.CanSit() || roomTile.CanLay()) return null;

            return roomTile;
        }

        public void AddRoomObjects(params IRoomObject[] roomObjects)
        {
            if (roomObjects.Length <= 0) return;

            IList<IRoomObject> userObjects = new List<IRoomObject>();
            IList<IRoomObject> furnitureObjects = new List<IRoomObject>();
            IList<IPoint> points = new List<IPoint>();

            foreach(IRoomObject roomObject in roomObjects)
            {
                if(roomObject.Logic is IFurnitureLogic furnitureLogic)
                {
                    IList<IPoint> affectedPoints = AffectedPoints.GetPoints(roomObject);

                    if(affectedPoints.Count > 0)
                    {
                        foreach(IPoint affectedPoint in affectedPoints)
                        {
                            IRoomTile roomTile = GetTile(affectedPoint);

                            if(roomTile != null)
                            {
                                roomTile.AddRoomObject(roomObject);

                                points.Add(affectedPoint);
                            }
                        }
                    }

                    furnitureObjects.Add(roomObject);
                }

                else if(roomObject.Logic is IMovingAvatarLogic avatarLogic)
                {
                    IRoomTile roomTile = GetTile(roomObject.Location);

                    if (roomTile != null)
                    {
                        roomTile.AddRoomObject(roomObject);
                    }

                    avatarLogic.InvokeCurrentLocation();

                    roomObject.NeedsUpdate = false;

                    userObjects.Add(roomObject);
                }
            }

            if (!_room.IsInitialized) return;

            if (furnitureObjects.Count > 0)
            {
                if(furnitureObjects.Count == 1)
                {
                    _room.SendComposer(new ObjectAddMessage
                    {
                        Object = furnitureObjects[0]
                    });
                }
                else
                {
                    _room.SendComposer(new ObjectsMessage
                    {
                        Objects = furnitureObjects
                    });
                }
            }

            if(userObjects.Count > 0)
            {
                _room.SendComposer(new UsersMessage
                {
                    RoomObjects = userObjects
                });

                _room.SendComposer(new UserUpdateMessage
                {
                    RoomObjects = userObjects
                });
            }
        }

        public void UpdatePoints(bool updateUsers = true, params IPoint[] points)
        {
            if (points.Length <= 0) return;

            List<IRoomTile> roomTiles = new();
            List<IRoomObject> roomObjects = new();

            foreach (IPoint point in points)
            {
                IRoomTile roomTile = GetTile(point);

                if ((roomTile == null) || roomTiles.Contains(roomTile)) continue;

                if(updateUsers && roomTile.Users.Count > 0)
                {
                    foreach (IRoomObject roomObject in roomTile.Users.Values)
                    {
                        if (roomObject.Logic is MovingAvatarLogic avatarLogic)
                        {
                            avatarLogic.InvokeCurrentLocation();

                            if (roomObject.NeedsUpdate)
                            {
                                roomObject.NeedsUpdate = false;

                                roomObjects.Add(roomObject);
                            }
                        }
                    }
                }

                roomTiles.Add(roomTile);

                // check roomTiles count, if count equals max byte length, send height packet and clear the list
            }

            if(roomTiles.Count > 0)
            {
                _room.SendComposer(new HeightMapUpdateMessage
                {
                    TilesToUpdate = roomTiles
                });
            }

            if(roomObjects.Count > 0)
            {
                _room.SendComposer(new UserUpdateMessage
                {
                    RoomObjects = roomObjects
                });
            }
        }
    }
}
