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
    public class NavigatorManager : Component, INavigatorManager
    {
        private readonly IRoomManager _roomManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<INavigatorManager> _logger;

        private readonly IDictionary<int, IPendingRoomInfo> _pendingRoomIds;

        public NavigatorManager(
            IRoomManager roomManager,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<INavigatorManager> logger)
        {
            _roomManager = roomManager;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;

            _pendingRoomIds = new Dictionary<int, IPendingRoomInfo>();
        }

        protected override async Task OnInit()
        {
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

            IRoom room = await _roomManager.GetRoom(roomId);

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
                    if (room.RoomDetails.State == RoomStateType.Locked)
                    {
                        ClearPendingRoomId(player.Id);

                        // doorbell
                        // if rights do u need 2 wait
                    }

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

        public async Task SendNavigatorMetaData(IPlayer player) => await player.Session.Send(new NavigatorMetaDataMessage
        {
            TopLevelContexts = new List<TopLevelContext>
            {
                new TopLevelContext { SearchCode = "official_view" },
                new TopLevelContext { SearchCode = "hotel_view" },
                new TopLevelContext { SearchCode = "roomads_view" },
                new TopLevelContext { SearchCode = "myworld_view" }
            }
        });

        public async Task SendNavigatorLiftedRooms(IPlayer player) => await player.Session.Send(new NavigatorLiftedRoomsMessage());

        public async Task SendNavigatorSavedSearches(IPlayer player) => await player.Session.Send(new NavigatorSavedSearchesMessage
        {
            // Todo: Implement saved searches
            SavedSearches = new List<NavigatorSavedSearch>(0)
        });

        public async Task SendNavigatorEventCategories(IPlayer player)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var categoriesRepo = scope.ServiceProvider.GetRequiredService<INavigatorEventCategoryRepository>();
                var categories = await categoriesRepo.FindAllAsync();

                await player.Session.Send(new NavigatorEventCategoriesMessage
                {
                    EventCategories = categories
                });
            }
        }
    }
}
