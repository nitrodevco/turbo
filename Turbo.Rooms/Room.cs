using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Events;
using Turbo.Core.Game;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Room;
using Turbo.Events.Game.Rooms.Avatar;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Cycles;
using Turbo.Rooms.Factories;
using Turbo.Rooms.Managers;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public class Room : Component, IRoom
    {
        public ILogger<IRoom> Logger { get; private set; }
        public IRoomManager RoomManager { get; private set; }
        public IRoomDetails RoomDetails { get; private set; }
        public IRoomCycleManager RoomCycleManager { get; private set; }
        public IRoomSecurityManager RoomSecurityManager { get; private set; }
        public IRoomFurnitureManager RoomFurnitureManager { get; private set; }
        public IRoomUserManager RoomUserManager { get; private set; }

        public IRoomModel RoomModel { get; private set; }
        public IRoomMap RoomMap { get; private set; }
        public IRoomChatManager RoomChatManager { get; private set;}

        private readonly ITurboEventHub _eventHub;
        private readonly IList<ISession> _roomObservers = new List<ISession>();
        private object _roomObserverLock = new();
        private int _remainingDisposeTicks = -1;

        public Room(
            ILogger<IRoom> logger,
            IRoomManager roomManager,
            RoomEntity roomEntity,
            IRoomSecurityFactory roomSecurityFactory,
            IRoomFurnitureFactory roomFurnitureFactory,
            IRoomUserFactory roomUserFactory,
            IChatFactory chatFactory,
            ITurboEventHub eventHub)
        {
            RoomManager = roomManager;
            Logger = logger;
            RoomDetails = new RoomDetails(this, roomEntity);

            RoomCycleManager = new RoomCycleManager(this);
            RoomSecurityManager = roomSecurityFactory.Create(this);
            RoomFurnitureManager = roomFurnitureFactory.Create(this);
            RoomUserManager = roomUserFactory.Create(this);
            RoomChatManager = chatFactory.Create(this);

            _eventHub = eventHub;

            RoomCycleManager.AddCycle(new RoomObjectCycle(this));
            RoomCycleManager.AddCycle(new RoomRollerCycle(this));
            RoomCycleManager.AddCycle(new RoomUserStatusCycle(this));
        }

        protected override async Task OnInit()
        {
            await LoadMapping();

            await RoomSecurityManager.InitAsync();
            await RoomFurnitureManager.InitAsync();
            await RoomUserManager.InitAsync();

            RoomCycleManager.Start();
        }

        protected override async Task OnDispose()
        {
            _remainingDisposeTicks = -1;

            RoomCycleManager.Stop();

            await RoomManager.RemoveRoom(Id);

            RoomCycleManager.Dispose();

            await RoomUserManager.DisposeAsync();
            await RoomFurnitureManager.DisposeAsync();
            await RoomSecurityManager.DisposeAsync();
        }

        public void TryDispose()
        {
            if (IsDisposed || IsDisposing) return;

            if (_remainingDisposeTicks != -1) return;

            if (RoomDetails.UsersNow > 0) return;

            RoomCycleManager.Stop();

            // clear the users waiting at the door

            _remainingDisposeTicks = DefaultSettings.RoomDisposeTicks;
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

            IRoomModel roomModel = await RoomManager.GetModel(RoomDetails.ModelId);

            if ((roomModel == null) || (!roomModel.IsValid)) return;

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

            if(RoomDetails.PaintWall != 0.0)
            {
                player.Session.SendQueue(new RoomPropertyMessage
                {
                    Property = RoomPropertyType.WALLPAPER,
                    Value = RoomDetails.PaintWall.ToString()
                });
            }

            if (RoomDetails.PaintFloor != 0.0)
            {
                player.Session.SendQueue(new RoomPropertyMessage
                {
                    Property = RoomPropertyType.FLOOR,
                    Value = RoomDetails.PaintFloor.ToString()
                });
            }

            if (RoomDetails.PaintLandscape != 0.0)
            {
                player.Session.SendQueue(new RoomPropertyMessage
                {
                    Property = RoomPropertyType.LANDSCAPE,
                    Value = RoomDetails.PaintLandscape.ToString()
                });
            }

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

            AddObserver(player.Session);

            // apply muted from security

            var roomObject = RoomUserManager.EnterRoom(player, location);

            if (roomObject != null)
            {
                RoomSecurityManager.RefreshControllerLevel(roomObject);

                var message = _eventHub.Dispatch(new AvatarEnterRoomEvent
                {
                    Avatar = roomObject
                });

                if (message.IsCancelled)
                {
                    roomObject.Dispose();

                    return;
                }
            }
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

        public bool IsGroupRoom => false;
    }
}
