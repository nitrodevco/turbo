using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Navigator;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Session;
using Turbo.Packets.Shared.Navigator;
using Turbo.Rooms.Utils;

namespace Turbo.Navigator
{
    public class NavigatorManager(
        IRoomManager _roomManager,
        INavigatorRepository _navigatorRepository,
        ILogger<INavigatorManager> _logger) : Component, INavigatorManager
    {
        private readonly IList<INavigatorTab> _tabs = new List<INavigatorTab>();
        private readonly IDictionary<int, INavigatorCategory> _categories = new Dictionary<int, INavigatorCategory>();
        private readonly IDictionary<int,INavigatorEventCategory> _eventCategories = new Dictionary<int, INavigatorEventCategory>();
        private readonly IDictionary<int, IPendingRoomInfo> _pendingRoomIds = new Dictionary<int, IPendingRoomInfo>();

        protected override async Task OnInit()
        {
            await LoadNavigatorTabs();
            await LoadNavigatorCategories();
            await LoadNavigatorEventCategories();
        }

        protected override async Task OnDispose()
        {
        }

        public int GetPendingRoomId(int userId)
        {
            if (!_pendingRoomIds.ContainsKey(userId)) return -1;

            return _pendingRoomIds[userId].RoomId;
        }

        public void SetPendingRoomId(int userId, int roomId, bool approved = false)
        {
            if ((userId <= 0) || (roomId <= 0)) return;

            _pendingRoomIds.Remove(userId);
            _pendingRoomIds.Add(userId, new PendingRoomInfo
            {
                RoomId = roomId,
                Approved = approved
            });
        }

        public void ClearPendingRoomId(int userId)
        {
            _pendingRoomIds.Remove(userId);
        }

        public void ClearRoomStatus(IPlayer player)
        {
            if (player == null) return;

            ClearPendingDoorbell(player);

            int pendingRoomId = GetPendingRoomId(player.Id);

            if (pendingRoomId == -1) player.Session?.Send(new CloseConnectionMessage());
        }

        public async Task GetGuestRoomMessage(IPlayer player, int roomId, bool enterRoom = false, bool roomForward = false)
        {
            if (player == null) return;

            var room = await _roomManager.GetRoom(roomId);

            if (room == null) return;

            await player.Session.Send(new GetGuestRoomResultMessage
            {
                EnterRoom = enterRoom,
                Room = room,
                IsRoomForward = roomForward,
                IsStaffPick = false,
                IsGroupMember = false,
                AllInRoomMuted = false,
                CanMute = false
            });
        }

        public async Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false, IPoint location = null)
        {
            if ((player == null) || (roomId <= 0)) return;

            int pendingRoomId = GetPendingRoomId(player.Id);

            if (pendingRoomId == roomId) return;

            SetPendingRoomId(player.Id, roomId);

            player.ClearRoomObject();

            var room = await _roomManager.GetRoom(roomId);

            if (room != null) await room.InitAsync();

            if (room == null || (room.RoomModel == null))
            {
                ClearPendingRoomId(player.Id);

                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Closed,
                    Parameter = ""
                });

                return;
            }

            if (!room.RoomSecurityManager.IsOwner(player))
            {
                if (room.RoomSecurityManager.IsPlayerBanned(player))
                {
                    ClearPendingRoomId(player.Id);

                    await player.Session.Send(new CantConnectMessage
                    {
                        Reason = CantConnectReason.Banned,
                        Parameter = ""
                    });

                    return;
                }

                if (room.RoomDetails.UsersNow >= room.RoomDetails.UsersMax)
                {
                    ClearPendingRoomId(player.Id);

                    await player.Session.Send(new CantConnectMessage
                    {
                        Reason = CantConnectReason.Full,
                        Parameter = ""
                    });

                    return;
                }

                if (!skipState)
                {
                    #region RoomStateType.Locked
                    if (room.RoomDetails.State == RoomStateType.Locked)
                    {
                        ClearPendingRoomId(player.Id);

                        // doorbell
                        // if rights do u need 2 wait
                    }
                    #endregion

                    #region RoomStateType.Password
                    else if (room.RoomDetails.State == RoomStateType.Password)
                    {
                        if (!password.Equals(room.RoomDetails.Password))
                        {
                            ClearPendingRoomId(player.Id);

                            await player.Session.Send(new GenericErrorMessage
                            {
                                ErrorCode = RoomGenericErrorType.InvalidPassword
                            });

                            return;
                        }
                    }
                    #endregion

                    #region RoomStateType.Invisible
                    else if (room.RoomDetails.State == RoomStateType.Invisible)
                    {
                        if (room.RoomSecurityManager.GetControllerLevel(player) == RoomControllerLevel.None)
                        {
                            ClearPendingRoomId(player.Id);

                            await player.Session.Send(new CantConnectMessage
                            {
                                Reason = CantConnectReason.Closed,
                                Parameter = ""
                            });

                            return;
                        }
                    }
                    #endregion
                }
            }

            ClearPendingDoorbell(player);

            SetPendingRoomId(player.Id, roomId, true);

            if (location != null) _pendingRoomIds[player.Id].Location = new Point(location);

            await player.Session.Send(new OpenConnectionMessage());
            await player.Session.Send(new RoomReadyMessage
            {
                RoomId = room.Id,
                RoomType = room.RoomModel.Name
            });
        }

        public async Task ContinueEnteringRoom(IPlayer player)
        {
            if (player == null) return;

            if (!_pendingRoomIds.ContainsKey(player.Id) || !_pendingRoomIds[player.Id].Approved)
            {
                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Closed
                });

                return;
            }

            int roomId = _pendingRoomIds[player.Id].RoomId;

            var room = await _roomManager.GetRoom(roomId);

            if (room == null)
            {
                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Closed
                });
            }

            if (room != null)
            {
                await room.InitAsync();

                room.EnterRoom(player, _pendingRoomIds[player.Id].Location);
            }

            ClearPendingRoomId(player.Id);
        }

        public void ClearPendingDoorbell(IPlayer player)
        {
            if (player == null) return;

            // remove user from pending doorbells
        }

        public async Task SendNavigatorCategories(IPlayer player) => await player.Session.Send(new UserFlatCatsMessage
        {

        });

        public async Task SendNavigatorSettings(IPlayer player) => await player.Session.Send(new NewNavigatorPreferencesMessage
        {
            WindowX = 0,
            WindowY = 0,
            WindowWidth = 0,
            WindowHeight = 0,
            LeftPaneHidden = false,
            ResultMode = 0
        });

        public async Task SendNavigatorMetaData(IPlayer player)
        {
            List<ITopLevelContext> tabs = [];


            foreach (var tab in _tabs)
            {
                tabs.Add(tab.TopLevelContext);
            }

            await player.Session.Send(new NavigatorMetaDataMessage
            {
                TopLevelContexts = tabs
            });
        }

        public async Task SendNavigatorLiftedRooms(IPlayer player) => await player.Session.Send(new NavigatorLiftedRoomsMessage());

        public async Task SendNavigatorSavedSearches(IPlayer player) => await player.Session.Send(new NavigatorSavedSearchesMessage
        {
            // Todo: Implement saved searches
            SavedSearches = new List<NavigatorSavedSearch>(0)
        });

        public async Task SendNavigatorEventCategories(IPlayer player)
        {
            var categories = await _navigatorRepository.FindAllNavigatorEventCategoriesAsync();

            await player.Session.Send(new NavigatorEventCategoriesMessage
            {
                EventCategories = categories
            });
        }

        private async Task LoadNavigatorTabs()
        {
            _tabs.Clear();

            var entities = await _navigatorRepository.FindAllNavigatorTabsAsync();

            entities.ForEach(entity =>
            {
                var tab = new NavigatorTab(entity);

                _tabs.Add(tab);
            });

            _logger.LogInformation("Loaded {0} navigator tabs", _tabs.Count);
        }

        private async Task LoadNavigatorCategories()
        {
            _categories.Clear();

            var entities = await _navigatorRepository.FindAllNavigatorCategoriesAsync();

            entities.ForEach(entity =>
            {
                var category = new NavigatorCategory(entity);

                _categories.Add(category.Id, category);
            });

            _logger.LogInformation("Loaded {0} navigator categories", _categories.Count);
        }

        private async Task LoadNavigatorEventCategories()
        {
            _eventCategories.Clear();

            var entities = await _navigatorRepository.FindAllNavigatorEventCategoriesAsync();

            entities.ForEach(entity =>
            {
                var eventCategory = new NavigatorEventCategory(entity);

                _eventCategories.Add(eventCategory.Id, eventCategory);
            });

            _logger.LogInformation("Loaded {0} navigator event categories", _eventCategories.Count);
        }
    }
}
