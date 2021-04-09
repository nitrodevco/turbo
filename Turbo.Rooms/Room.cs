using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Messages;
using Turbo.Database.Entities.Room;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Factories;
using Turbo.Rooms.Managers;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public class Room : IRoom
    {
        private static readonly int _disposeTicks = 120;

        public ILogger<IRoom> Logger { get; private set; }
        public IRoomManager RoomManager { get; private set; }
        public IRoomDetails RoomDetails { get; private set; }
        public IRoomCycleManager RoomCycleManager { get; private set; }
        public IRoomSecurityManager RoomSecurityManager { get; private set; }
        public IRoomFurnitureManager RoomFurnitureManager { get; private set; }
        public IRoomWiredManager RoomWiredManager { get; private set; }
        public IRoomUserManager RoomUserManager { get; private set; }

        public IRoomModel RoomModel { get; private set; }
        public IRoomMap RoomMap { get; private set; }


        private readonly IList<ISession> _roomObservers;
        private object _roomObserverLock = new();
        private int _remainingDisposeTicks = -1;

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public Room(
            ILogger<IRoom> logger,
            IRoomManager roomManager,
            RoomEntity roomEntity,
            IRoomSecurityFactory roomSecurityFactory,
            IRoomFurnitureFactory roomFurnitureFactory,
            IRoomWiredFactory roomWiredFactory,
            IRoomUserFactory roomUserFactory)
        {
            RoomManager = roomManager;
            Logger = logger;
            RoomDetails = new RoomDetails(this, roomEntity);

            RoomCycleManager = new RoomCycleManager(this);
            RoomSecurityManager = roomSecurityFactory.Create(this);
            RoomFurnitureManager = roomFurnitureFactory.Create(this);
            RoomWiredManager = roomWiredFactory.Create(this);
            RoomUserManager = roomUserFactory.Create(this);

            _roomObservers = new List<ISession>();
        }

        public async ValueTask InitAsync()
        {
            if (IsInitialized) return;

            await LoadMapping();

            if (RoomSecurityManager != null) await RoomSecurityManager.InitAsync();
            if (RoomFurnitureManager != null) await RoomFurnitureManager.InitAsync();
            if (RoomUserManager != null) await RoomUserManager.InitAsync();

            Logger.LogInformation("Room loaded");

            RoomCycleManager.Start();

            IsInitialized = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            CancelDispose();

            if (RoomManager != null) await RoomManager.RemoveRoom(Id);

            if (RoomCycleManager != null) RoomCycleManager.Dispose();
            if (RoomUserManager != null) await RoomUserManager.DisposeAsync();
            if (RoomFurnitureManager != null) await RoomFurnitureManager.DisposeAsync();
            if (RoomSecurityManager != null) await RoomSecurityManager.DisposeAsync();

            IsDisposed = true;
        }

        public void TryDispose()
        {
            if (IsDisposed || IsDisposing) return;

            if (_remainingDisposeTicks != -1) return;

            if (RoomDetails.UsersNow > 0) return;

            RoomCycleManager.Stop();

            // clear the users waiting at the door

            _remainingDisposeTicks = _disposeTicks;
        }

        public void CancelDispose()
        {
            RoomCycleManager.Start();

            _remainingDisposeTicks = -1;
        }

        private async Task LoadMapping()
        {
            if (RoomMap != null)
            {
                RoomMap.Dispose();

                RoomMap = null;
            }

            RoomModel = null;

            IRoomModel roomModel = RoomManager.GetModel(RoomDetails.ModelId);

            if ((roomModel == null) || (!roomModel.DidGenerate)) return;

            RoomModel = roomModel;
            RoomMap = new RoomMap(this);

            RoomMap.GenerateMap();
        }

        public void EnterRoom(IPlayer player, IPoint location = null)
        {
            if (player == null) return;

            player.Session.SendQueue(new HeightMapMessage
            {
                RoomModel = RoomModel,
                RoomMap = RoomMap
            });

            player.Session.SendQueue(new FloorHeightMapMessage
            {
                IsZoomedIn = true,
                WallHeight = RoomDetails.WallHeight,
                RoomModel = RoomModel
            });

            player.Session.SendQueue(new RoomVisualizationSettingsMessage
            {
                WallsHidden = RoomDetails.HideWalls,
                FloorThickness = (int)RoomDetails.ThicknessFloor,
                WallThickness = (int)RoomDetails.ThicknessWall
            });

            // send the paint

            // would be nice to send this from the navigator so we aren't duplicating code
            player.Session.SendQueue(new GetGuestRoomResultMessage
            {
                EnterRoom = true,
                Room = this,
                IsRoomForward = false,
                IsStaffPick = false,
                IsGroupMember = false,
                AllInRoomMuted = false,
                CanMute = false
            });

            player.Session.Flush();

            RoomFurnitureManager.SendFurnitureToSession(player.Session);

            IRoomObject roomObject = RoomUserManager.EnterRoom(player, location);

            if (roomObject != null) RoomSecurityManager.RefreshControllerLevel(roomObject);

            // apply muted from security

            AddObserver(player.Session);

            RoomWiredManager.ProcessTriggers(RoomObjectLogicType.FurnitureWiredTriggerEnterRoom);
        }

        public void AddObserver(ISession session)
        {
            lock (_roomObserverLock)
            {
                _roomObservers.Add(session);
            }
        }

        public void RemoveObserver(ISession session)
        {
            lock (_roomObserverLock)
            {
                _roomObservers.Remove(session);
            }
        }

        public async Task Cycle()
        {
            if (_remainingDisposeTicks > -1)
            {
                if (_remainingDisposeTicks == 0)
                {
                    await DisposeAsync();

                    _remainingDisposeTicks = -1;

                    return;
                }

                _remainingDisposeTicks--;
            }

            await RoomCycleManager.Cycle();
        }

        public void SendComposer(IComposer composer)
        {
            lock (_roomObserverLock)
            {
                foreach (ISession session in _roomObservers)
                {
                    session.Send(composer);
                }
            }
        }

        public int Id => RoomDetails.Id;
    }
}
