using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Turbo.Core.Game;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Cycles
{
    public class RoomRollerCycle : RoomCycle
    {
        private readonly static int _rollerCycles = 4;
        private readonly static double _maxFallingHeight = 0.5;

        private int _remainingRollerCycles = _rollerCycles;

        private List<IRollerData> _lastRollingDatas;
        private List<IPoint> _lastRollingPoints;

        public RoomRollerCycle(IRoom room) : base(room)
        {
            
        }

        public override async Task Cycle()
        {
            if (_remainingRollerCycles > -1)
            {
                if(_lastRollingDatas != null)
                {
                    CompleteLastRolls();
                }

                if (_remainingRollerCycles > 0)
                {
                    _remainingRollerCycles--;
                }

                if (_remainingRollerCycles != 0) return;

                _remainingRollerCycles = _rollerCycles;
            }

            IList<IRoomObject> rollers = _room.RoomFurnitureManager.GetRoomObjectsWithLogic(typeof(FurnitureRollerLogic));

            if (rollers.Count == 0) return;

            List<IRollerData> rollingDatas = new();

            foreach(IRoomObject roomObject in rollers)
            {
                IRollerData rollingData = new RollerData(_room, roomObject.Location, roomObject.Location.GetPointForward());

                rollingData.Roller = roomObject;

                if (!ProcessRollerData(rollingData)) continue;

                rollingDatas.Add(rollingData);
            }

            if (rollingDatas.Count == 0) return;

            List<IPoint> points = new();
            List<IComposer> composers = new();

            foreach(IRollerData rollingData in rollingDatas)
            {
                points.Add(rollingData.Location);
                points.Add(rollingData.LocationNext);

                if(rollingData.Users.Count > 0)
                {
                    bool sent = false;

                    foreach(IRollerItemData rollerItemData in rollingData.Users.Values)
                    {
                        if(!sent)
                        {
                            composers.Add(new SlideObjectBundleMessage
                            {
                                OldPos = rollingData.Location,
                                NewPos = rollingData.LocationNext,
                                User = rollerItemData,
                                Furniture = (rollingData.Furniture != null) ? rollingData.Furniture.Values.ToList() : null,
                                RollerItemId = rollingData.Roller.Id
                            });

                            sent = true;

                            continue;
                        }

                        composers.Add(new SlideObjectBundleMessage
                        {
                            OldPos = rollingData.Location,
                            NewPos = rollingData.LocationNext,
                            User = rollerItemData,
                            RollerItemId = rollingData.Roller.Id
                        });
                    }
                }
                else
                {
                    if (rollingData.Furniture.Count > 0)
                    {
                        composers.Add(new SlideObjectBundleMessage
                        {
                            OldPos = rollingData.Location,
                            NewPos = rollingData.LocationNext,
                            Furniture = rollingData.Furniture.Values.ToList(),
                            RollerItemId = rollingData.Roller.Id
                        });
                    }
                }

                rollingData.CommitRoll();
            }

            if (composers.Count > 0) foreach (IComposer composer in composers) _room.SendComposer(composer);

            _lastRollingDatas = rollingDatas;
            _lastRollingPoints = points;
        }

        private void CompleteLastRolls()
        {
            if ((_lastRollingPoints != null) && (_lastRollingPoints.Count > 0))
            {
                _room.RoomMap.UpdatePoints(true, _lastRollingPoints.ToArray());

                _lastRollingPoints = null;
            }

            foreach(IRollerData rollerData in _lastRollingDatas)
            {
                rollerData.CompleteRoll();
            }

            _lastRollingDatas = null;
        }

        private bool ProcessRollerData(IRollerData rollingData)
        {
            IRoomTile roomTile = _room.RoomMap.GetTile(rollingData.Location);

            if (roomTile == null) return false;

            foreach (IRoomObject roomObject in roomTile.Furniture.Values)
            {
                if ((rollingData.Roller == roomObject) || (rollingData.Furniture.ContainsKey(roomObject.Id))) continue;

                ProcessRollingFurniture(rollingData, roomObject);
            }

            foreach (IRoomObject roomObject in roomTile.Users.Values)
            {
                if (rollingData.Users.ContainsKey(roomObject.Id)) continue;

                ProcessRollingUser(rollingData, roomObject);
            }

            if ((rollingData.Users.Count == 0) && (rollingData.Furniture.Count == 0)) return false;

            return true;
        }

        private bool ProcessRollingUser(IRollerData rollingData, IRoomObject roomObject, bool validateOnly = false)
        {
            if ((roomObject.Logic is not IMovingAvatarLogic movingAvatarLogic) || movingAvatarLogic.IsWalking) return false;

            if (!roomObject.Location.Compare(rollingData.Location)) return false;

            if(rollingData.Roller != null)
            {
                if (roomObject.Location.Z < ((IFurnitureLogic)rollingData.Roller.Logic).Height) return false;
            }

            IRoomTile nextTile = _room.RoomMap.GetValidTile(roomObject, rollingData.LocationNext);

            if (nextTile == null) return false;

            if(nextTile.HighestObject != null && nextTile.HighestObject.Logic is IFurnitureLogic nextTileLogic)
            {
                if ((nextTile.Height - nextTileLogic.StackHeight) > roomObject.Location.Z) return false;
            }

            foreach(IRoomObject userObject in _room.RoomUserManager.RoomObjects.Values)
            {
                if ((userObject == roomObject) || (userObject.Logic is not IMovingAvatarLogic avatarLogic)) continue;

                if (avatarLogic.LocationNext != null)
                {
                    if (avatarLogic.LocationNext.Compare(rollingData.LocationNext)) return false;
                }

                if (avatarLogic.IsWalking) continue;

                if (avatarLogic.IsRolling)
                {
                    if (avatarLogic.RollerData == rollingData) continue;

                    if (avatarLogic.RollerData.LocationNext.Compare(rollingData.LocationNext)) return false;
                }

                if (userObject.Location.Compare(rollingData.LocationNext)) return false;
            }

            foreach (IRoomObject furnitureObject in _room.RoomFurnitureManager.RoomObjects.Values)
            {
                if (furnitureObject.Logic is not IFurnitureLogic furnitureLogic || (furnitureObject.Logic is FurnitureRollerLogic)) continue;

                if (!furnitureLogic.IsRolling) continue;

                if (furnitureObject.Location.Compare(rollingData.Roller.Location)) continue;

                if (furnitureLogic.RollerData.LocationNext.Compare(rollingData.LocationNext)) return false;
            }

            IRoomTile currentTile = movingAvatarLogic.GetCurrentTile();

            if (currentTile == null) return false;

            if (currentTile.HighestObject != null && currentTile.HighestObject.Logic is IFurnitureLogic)
            {
                if (currentTile.HighestObject.Logic is not FurnitureRollerLogic)
                {
                    if (!ProcessRollingFurniture(rollingData, currentTile.HighestObject, true)) return false;
                }
            }

            if (validateOnly) return true;

            double nextHeight = nextTile.GetWalkingHeight();

            rollingData.Users.Add(roomObject.Id, new RollerItemData
            {
                RoomObject = roomObject,
                Height = roomObject.Location.Z,
                HeightNext = nextHeight
            });

            movingAvatarLogic.RollerData = rollingData;

            return true;
        }

        private bool ProcessRollingFurniture(IRollerData rollingData, IRoomObject roomObject, bool validateOnly = false)
        {
            if (!roomObject.Location.Compare(rollingData.Location)) return false;

            if (roomObject.Logic is not IFurnitureLogic rollingObjectLogic) return false;

            if (!rollingObjectLogic.CanRoll()) return false;

            if (rollingData.Roller != null)
            {
                if (roomObject.Location.Z < ((IFurnitureLogic)rollingData.Roller.Logic).Height) return false;
            }

            if (!_room.RoomFurnitureManager.IsValidPlacement(roomObject, rollingData.LocationNext)) return false;

            foreach (IRoomObject userObject in _room.RoomUserManager.RoomObjects.Values)
            {
                if (userObject.Logic is not IMovingAvatarLogic avatarLogic) continue;

                if (avatarLogic.LocationNext != null)
                {
                    if (avatarLogic.LocationNext.Compare(rollingData.LocationNext)) return false;
                }

                if (avatarLogic.IsWalking) continue;

                if (avatarLogic.IsRolling)
                {
                    if (avatarLogic.RollerData == rollingData) continue;

                    if (avatarLogic.RollerData.LocationNext.Compare(rollingData.LocationNext)) return false;
                }

                if (userObject.Location.Compare(rollingData.LocationNext)) return false;
            }

            foreach (IRoomObject furnitureObject in _room.RoomFurnitureManager.RoomObjects.Values)
            {
                if (furnitureObject.Logic is not IFurnitureLogic furnitureLogic || (furnitureObject == roomObject) || (furnitureObject.Logic is FurnitureRollerLogic)) continue;

                if (furnitureLogic.IsRolling)
                {
                    if (furnitureLogic.RollerData == rollingData) continue;

                    if (furnitureLogic.RollerData.LocationNext.Compare(rollingData.LocationNext)) return false;
                }

                if (furnitureObject.Location.Compare(rollingData.LocationNext)) return false;
            }

            if (validateOnly) return true;

            double nextHeight = roomObject.Location.Z;

            if (rollingData.Roller != null)
            {
                IRoomTile roomTileNext = _room.RoomMap.GetTile(rollingData.LocationNext);

                if (roomTileNext == null) return false;

                if (!roomTileNext.HasLogic(typeof(FurnitureRollerLogic)))
                {
                    nextHeight -= ((IFurnitureLogic)rollingData.Roller.Logic).StackHeight;
                }
            }
            else
            {
                IRoomTile roomTile = _room.RoomMap.GetTile(rollingData.Location);

                if (roomTile != null) nextHeight = roomTile.Height;
            }

            rollingData.Furniture.Add(roomObject.Id, new RollerItemData
            {
                RoomObject = roomObject,
                Height = roomObject.Location.Z,
                HeightNext = nextHeight
            });

            rollingObjectLogic.RollerData = rollingData;

            return true;
        }
    }
}
