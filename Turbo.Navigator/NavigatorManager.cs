using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Session;

namespace Turbo.Navigator
{
    public class NavigatorManager : INavigatorManager
    {
        private readonly IRoomManager _roomManager;
        private readonly ILogger<INavigatorManager> _logger;

        private readonly IDictionary<int, int> _pendingRoomIds;

        public NavigatorManager(
            IRoomManager roomManager,
            ILogger<INavigatorManager> logger)
        {
            _roomManager = roomManager;
            _logger = logger;

            _pendingRoomIds = new Dictionary<int, int>();
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }

        public int GetPendingRoomId(int userId)
        {
            if (!_pendingRoomIds.ContainsKey(userId)) return -1;

            return _pendingRoomIds[userId];
        }

        public void SetPendingRoomId(int userId, int roomId)
        {
            if ((userId <= 0) || (roomId <= 0)) return;

            _pendingRoomIds.Add(userId, roomId);
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

            if (pendingRoomId == -1)
            {
                if (player.Session != null) player.Session.Send(new CloseConnectionMessage());
            }
        }

        public async Task GetGuestRoomMessage(IPlayer player, int roomId, bool enterRoom = false, bool isRoomForward = false)
        {
            if (player == null) return;

            IRoom room = await _roomManager.GetRoom(roomId);

            if (room == null) return;

            await player.Session.Send(new GetGuestRoomResultMessage
            {
                EnterRoom = enterRoom,
                Room = room,
                IsRoomForward = isRoomForward,
                IsStaffPick = false,
                IsGroupMember = false,
                AllInRoomMuted = false,
                CanMute = false
            });
        }

        public async Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false)
        {
            if ((player == null) || (roomId <= 0)) return;

            int pendingRoomId = GetPendingRoomId(player.Id);

            if (pendingRoomId == roomId) return;

            player.ClearRoomObject();

            IRoom room = await _roomManager.GetRoom(roomId);

            if (room != null) await room.InitAsync();

            if (room == null || (room.RoomModel == null))
            {
                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Closed,
                    Parameter = ""
                });

                return;
            }

            // check if banned

            // if not owner

            // if usersNow >= usersMax
            // if !skipsState
            // if locked
            // request doorbell
            // if password
            // test the password
            // if invisible
            // check rights

            // if locked state clear the doorbell

            SetPendingRoomId(player.Id, roomId);

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

            int roomId = GetPendingRoomId(player.Id);

            if(roomId == -1)
            {
                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Closed
                });
            }
            else
            {
                IRoom room = await _roomManager.GetRoom(roomId);

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

                    room.EnterRoom(player);
                }
            }

            ClearPendingRoomId(player.Id);
        }

        public void ClearPendingDoorbell(IPlayer player)
        {
            if (player == null) return;

            // remove user from pending doorbells
        }
    }
}
