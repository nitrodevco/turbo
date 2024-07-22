using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Cycles;

public class RoomRollerCycle(IRoom _room) : RoomCycle(_room)
{
    private List<IRollerData> _lastRollingDatas;
    private List<IPoint> _lastRollingPoints;
    private int _remainingRollerCycles = DefaultSettings.RollerCycles;

    public override async Task Cycle()
    {
        await Task.Run(() =>
        {
            if (_remainingRollerCycles > -1)
            {
                if (_lastRollingDatas != null) CompleteLastRolls();

                if (_remainingRollerCycles > 0) _remainingRollerCycles--;

                if (_remainingRollerCycles != 0) return;

                _remainingRollerCycles = DefaultSettings.RollerCycles;
            }

            var rollers = _room.RoomFurnitureManager.GetFloorRoomObjectsWithLogic(typeof(FurnitureRollerLogic));

            if (rollers.Count == 0) return;

            List<IRollerData> rollingDatas = new();

            foreach (var floorObject in rollers)
            {
                var rollingData = new RollerData(_room, floorObject.Location, floorObject.Location.GetPointForward());

                rollingData.Roller = floorObject;

                if (!ProcessRollerData(rollingData)) continue;

                rollingDatas.Add(rollingData);
            }

            if (rollingDatas.Count == 0) return;

            List<IPoint> points = new();
            List<IComposer> composers = new();

            foreach (var rollingData in rollingDatas)
            {
                points.Add(rollingData.Location);
                points.Add(rollingData.LocationNext);

                if (rollingData.Avatars.Count > 0)
                {
                    var sent = false;

                    foreach (var rollerItemData in rollingData.Avatars.Values)
                    {
                        if (!sent)
                        {
                            composers.Add(new SlideObjectBundleMessage
                            {
                                OldPos = rollingData.Location,
                                NewPos = rollingData.LocationNext,
                                Avatar = rollerItemData,
                                Furniture =
                                    rollingData.Furniture != null ? rollingData.Furniture.Values.ToList() : null,
                                RollerItemId = rollingData.Roller.Id
                            });

                            sent = true;

                            continue;
                        }

                        composers.Add(new SlideObjectBundleMessage
                        {
                            OldPos = rollingData.Location,
                            NewPos = rollingData.LocationNext,
                            Avatar = rollerItemData,
                            RollerItemId = rollingData.Roller.Id
                        });
                    }
                }
                else
                {
                    if (rollingData.Furniture.Count > 0)
                        composers.Add(new SlideObjectBundleMessage
                        {
                            OldPos = rollingData.Location,
                            NewPos = rollingData.LocationNext,
                            Furniture = rollingData.Furniture.Values.ToList(),
                            RollerItemId = rollingData.Roller.Id
                        });
                }

                rollingData.CommitRoll();
            }

            if (composers.Count > 0)
                foreach (var composer in composers)
                    _room.SendComposer(composer);

            _lastRollingDatas = rollingDatas;
            _lastRollingPoints = points;
        });
    }

    private void CompleteLastRolls()
    {
        if (_lastRollingPoints != null && _lastRollingPoints.Count > 0)
        {
            _room.RoomMap.UpdatePoints(true, _lastRollingPoints.ToArray());

            _lastRollingPoints = null;
        }

        foreach (var rollerData in _lastRollingDatas) rollerData.CompleteRoll();

        _lastRollingDatas = null;
    }

    private bool ProcessRollerData(IRollerData rollingData)
    {
        var roomTile = _room.RoomMap.GetTile(rollingData.Location);

        if (roomTile == null) return false;

        foreach (var floorObject in roomTile.Furniture)
        {
            if (rollingData.Roller == floorObject || rollingData.Furniture.ContainsKey(floorObject.Id)) continue;

            ProcessRollingFurniture(rollingData, floorObject);
        }

        foreach (var avatarObject in roomTile.Avatars)
        {
            if (rollingData.Avatars.ContainsKey(avatarObject.Id)) continue;

            ProcessRollingAvatar(rollingData, avatarObject);
        }

        if (rollingData.Avatars.Count == 0 && rollingData.Furniture.Count == 0) return false;

        return true;
    }

    private bool ProcessRollingAvatar(IRollerData rollingData, IRoomObjectAvatar avatarObject,
        bool validateOnly = false)
    {
        if (avatarObject.Logic.IsWalking) return false;

        if (!avatarObject.Location.Compare(rollingData.Location)) return false;

        if (rollingData.Roller != null)
            if (avatarObject.Location.Z < rollingData.Roller.Logic.Height)
                return false;

        var nextTile = _room.RoomMap.GetValidTile(avatarObject, rollingData.LocationNext);

        if (nextTile == null) return false;

        if (nextTile.HighestObject != null)
            if (nextTile.Height - nextTile.HighestObject.Logic.StackHeight > avatarObject.Location.Z)
                return false;

        foreach (var existingAvatarObject in _room.RoomUserManager.AvatarObjects.RoomObjects.Values)
        {
            if (existingAvatarObject == avatarObject) continue;

            if (existingAvatarObject.Logic.LocationNext != null)
                if (existingAvatarObject.Logic.LocationNext.Compare(rollingData.LocationNext))
                    return false;

            if (existingAvatarObject.Logic.IsWalking) continue;

            if (existingAvatarObject.Logic.IsRolling)
            {
                if (existingAvatarObject.Logic.RollerData == rollingData) continue;

                if (existingAvatarObject.Logic.RollerData.LocationNext.Compare(rollingData.LocationNext)) return false;
            }

            if (existingAvatarObject.Location.Compare(rollingData.LocationNext)) return false;
        }

        foreach (var existingFloorObject in _room.RoomFurnitureManager.FloorObjects.RoomObjects.Values)
        {
            if (existingFloorObject.Logic is FurnitureRollerLogic) continue;

            if (!existingFloorObject.Logic.IsRolling) continue;

            if (existingFloorObject.Location.Compare(rollingData.Roller.Location)) continue;

            if (existingFloorObject.Logic.RollerData.LocationNext.Compare(rollingData.LocationNext)) return false;
        }

        var currentTile = avatarObject.Logic.GetCurrentTile();

        if (currentTile == null) return false;

        if (currentTile.HighestObject != null)
            if (currentTile.HighestObject.Logic is not FurnitureRollerLogic)
                if (!ProcessRollingFurniture(rollingData, currentTile.HighestObject, true))
                    return false;

        if (validateOnly) return true;

        var nextHeight = nextTile.GetWalkingHeight();

        rollingData.Avatars.Add(avatarObject.Id, new RollerItemData<IRoomObjectAvatar>
        {
            RoomObject = avatarObject,
            Height = avatarObject.Location.Z,
            HeightNext = nextHeight
        });

        avatarObject.Logic.RollerData = rollingData;

        return true;
    }

    private bool ProcessRollingFurniture(IRollerData rollingData, IRoomObjectFloor floorObject,
        bool validateOnly = false)
    {
        if (!floorObject.Location.Compare(rollingData.Location)) return false;

        if (!floorObject.Logic.CanRoll()) return false;

        if (rollingData.Roller != null)
            if (floorObject.Location.Z < rollingData.Roller.Logic.Height)
                return false;

        if (!_room.RoomFurnitureManager.IsValidPlacement(floorObject, rollingData.LocationNext)) return false;

        foreach (var existingAvatarObject in _room.RoomUserManager.AvatarObjects.RoomObjects.Values)
        {
            if (existingAvatarObject.Logic.LocationNext != null)
                if (existingAvatarObject.Logic.LocationNext.Compare(rollingData.LocationNext))
                    return false;

            if (existingAvatarObject.Logic.IsWalking) continue;

            if (existingAvatarObject.Logic.IsRolling)
            {
                if (existingAvatarObject.Logic.RollerData == rollingData) continue;

                if (existingAvatarObject.Logic.RollerData.LocationNext.Compare(rollingData.LocationNext)) return false;
            }

            if (existingAvatarObject.Location.Compare(rollingData.LocationNext)) return false;
        }

        foreach (var existingFurnitureObject in _room.RoomFurnitureManager.FloorObjects.RoomObjects.Values)
        {
            if (existingFurnitureObject == floorObject ||
                existingFurnitureObject.Logic is FurnitureRollerLogic) continue;

            if (existingFurnitureObject.Logic.IsRolling)
            {
                if (existingFurnitureObject.Logic.RollerData == rollingData) continue;

                if (existingFurnitureObject.Logic.RollerData.LocationNext.Compare(rollingData.LocationNext))
                    return false;
            }

            if (existingFurnitureObject.Location.Compare(rollingData.LocationNext)) return false;
        }

        if (validateOnly) return true;

        var nextHeight = floorObject.Location.Z;

        if (rollingData.Roller != null)
        {
            var roomTileNext = _room.RoomMap.GetTile(rollingData.LocationNext);

            if (roomTileNext == null) return false;

            if (!roomTileNext.HasLogic(typeof(FurnitureRollerLogic)))
                nextHeight -= rollingData.Roller.Logic.StackHeight;
        }
        else
        {
            var roomTile = _room.RoomMap.GetTile(rollingData.Location);

            if (roomTile != null) nextHeight = roomTile.Height;
        }

        rollingData.Furniture.Add(floorObject.Id, new RollerItemData<IRoomObjectFloor>
        {
            RoomObject = floorObject,
            Height = floorObject.Location.Z,
            HeightNext = nextHeight
        });

        floorObject.Logic.RollerData = rollingData;

        return true;
    }
}