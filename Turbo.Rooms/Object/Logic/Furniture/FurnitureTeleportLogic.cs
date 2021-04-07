using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureTeleportLogic : FurnitureLogic
    {
        private static readonly int _closedState = 0;
        private static readonly int _openState = 1;
        private static readonly int _animatingState = 2;

        private TeleportPairingDto _dto;
        private IRoomObject _pendingRoomObject;
        private IPlayer _pendingPlayer;
        private bool _didFindTeleport;
        private bool _needsOpening;
        private bool _needsClosing;
        private bool _needsAnimating;

        public override async Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
        {
            if (!await base.Setup(furnitureDefinition, jsonString)) return false;

            SetState(_closedState);

            if(_dto == null)
            {
                if (RoomObject.RoomObjectHolder is IFurniture furniture)
                {
                    _dto = await furniture.GetTeleportPairing();
                }
            }

            return true;
        }

        public override void Dispose()
        {
            if(_pendingRoomObject != null)
            {
                if(_pendingRoomObject.Logic is IMovingAvatarLogic avatarLogic)
                {
                    avatarLogic.CanWalk = true;

                    _pendingRoomObject = null;
                }
            }

            if (_pendingPlayer != null) _pendingPlayer = null;

            base.Dispose();
        }

        public override async Task Cycle()
        {
            await base.Cycle();

            if(_needsOpening)
            {
                SetState(_openState);

                _needsOpening = false;
            }

            if (_needsClosing)
            {
                SetState(_closedState);

                _needsClosing = false;
            }

            if(_needsAnimating)
            {
                SetState(_animatingState);

                _needsAnimating = false;
            }

            if(_pendingPlayer != null)
            {
                await ReceiveTeleport(_pendingPlayer);
            }

            if (_pendingRoomObject == null) return;

            if(_pendingRoomObject.Disposed || (_pendingRoomObject.Logic is not IMovingAvatarLogic avatarLogic))
            {
                SetState(_closedState);

                _pendingRoomObject = null;

                return;
            }

            if (!_pendingRoomObject.Location.Compare(RoomObject.Location))
            {
                if (avatarLogic.IsWalking && avatarLogic.LocationGoal.Compare(RoomObject.Location)) return;

                avatarLogic.CanWalk = true;

                SetState(_closedState);

                _pendingRoomObject = null;
                
                return;
            }

            IRoom remoteRoom = null;

            if(_dto != null && _dto.RoomId != null)
            {
                remoteRoom = await RoomObject.Room.RoomManager.GetRoom((int)_dto.RoomId);
            }

            bool didFail = false;

            if(_dto == null || remoteRoom == null)
            {
                didFail = true;
            }
            else
            {
                if(remoteRoom.Id != RoomObject.Room.Id) await remoteRoom.InitAsync();

                int state = StuffData.GetState();

                if(state == _openState)
                {
                    _needsClosing = true;

                    return;
                }

                else if(state == _closedState)
                {
                    _needsAnimating = true;

                    return;
                }

                else if(state == _animatingState)
                {
                    IRoomObject remoteFurniture = remoteRoom.RoomFurnitureManager.GetRoomObject(_dto.TeleportId);

                    if(!_didFindTeleport)
                    {
                        if(remoteFurniture == null)
                        {
                            didFail = true;
                        }
                        else
                        {
                            _didFindTeleport = true;

                            return;
                        }
                    }

                    if (remoteFurniture == null)
                    {
                        didFail = true;
                    }

                    else if (remoteFurniture.Logic is FurnitureTeleportLogic teleportLogic)
                    {
                        if(_pendingRoomObject != null && teleportLogic.StuffData.GetState() != _animatingState)
                        {
                            teleportLogic.SetState(_animatingState);

                            return;
                        }
                        
                        SetState(_closedState);

                        if(_pendingRoomObject.RoomObjectHolder is IPlayer player)
                        {
                            await teleportLogic.ReceiveTeleport(player);
                        }

                        _pendingRoomObject = null;
                    }

                    else
                    {
                        didFail = true;
                    }
                }
            }

            if(didFail)
            {
                _pendingRoomObject = null;

                avatarLogic.WalkTo(RoomObject.Location.GetPointForward());

                avatarLogic.CanWalk = true;

                _needsClosing = true;

                return;
            }
        }

        private async Task ReceiveTeleport(IPlayer player)
        {
            if (player.RoomObject == null || player.RoomObject.Disposed)
            {
                _pendingRoomObject = null;

                _needsClosing = true;

                return;
            }

            _pendingPlayer = player;

            if (player.RoomObject != null)
            {
                if(player.RoomObject.Room != RoomObject.Room)
                {
                    await player.PlayerManager.EnterRoom(player, RoomObject.Room.Id, null, true, RoomObject.Location);
                }
                else
                {
                    if (player.RoomObject == null || player.RoomObject.Disposed || player.RoomObject.Logic is not IMovingAvatarLogic avatarLogic)
                    {
                        _pendingPlayer = null;

                        _needsClosing = true;

                        return;
                    }

                    avatarLogic.CanWalk = false;

                    if (player.RoomObject.Location.Compare(RoomObject.Location))
                    {
                        if (StuffData.GetState() != _openState)
                        {
                            _needsOpening = true;

                            return;
                        }

                        if (StuffData.GetState() == _openState)
                        {
                            avatarLogic.WalkTo(RoomObject.Location.GetPointForward());

                            avatarLogic.CanWalk = true;

                            _pendingPlayer = null;
                        }
                        
                        _needsClosing = true;
                    }
                    else
                    {
                        avatarLogic.GoTo(RoomObject.Location);
                    }
                }
            }
        }

        public override void OnInteract(IRoomObject roomObject, int param = 0)
        {
            if (_pendingRoomObject != null && roomObject != _pendingRoomObject) return;

            if (roomObject.Logic is not IMovingAvatarLogic avatarLogic) return;

            IPoint goalPoint = GetGoalPoint();

            if (!roomObject.Location.Compare(goalPoint))
            {
                avatarLogic.WalkTo(goalPoint, true);

                avatarLogic.BeforeGoalAction = new Action<IRoomObject>(p => OnInteract(p));

                return;
            }

            SetState(_openState);

            avatarLogic.CanWalk = false;

            _pendingRoomObject = roomObject;

            if (roomObject.Location.Compare(RoomObject.Location)) return;

            avatarLogic.WalkTo(RoomObject.Location);
        }

        public override void OnPickup(IRoomManipulator roomManipulator)
        {
            SetState(_closedState);

            base.OnPickup(roomManipulator);
        }

        public override bool CanWalk(IRoomObject roomObject = null)
        {
            if ((_pendingRoomObject != null) && roomObject == _pendingRoomObject) return true;

            return base.CanWalk(roomObject);
        }

        private IPoint GetGoalPoint()
        {
            IPoint goalPoint;

            if (CanWalk())
            {
                goalPoint = RoomObject.Location;
            }
            else
            {
                goalPoint = RoomObject.Location.GetPointForward();
            }

            return goalPoint;
        }

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Everybody;
    }
}
