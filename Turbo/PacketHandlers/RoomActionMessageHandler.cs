using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Action;

namespace Turbo.PacketHandlers
{
    public class RoomActionMessageHandler : IRoomActionMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IRoomManager _roomManager;

        public RoomActionMessageHandler(
            IPacketMessageHub messageHub,
            IRoomManager roomManager)
        {
            _messageHub = messageHub;
            _roomManager = roomManager;

            _messageHub.Subscribe<AmbassadorAlertMessage>(this, OnAmbassadorAlertMessage);
            _messageHub.Subscribe<AssignRightsMessage>(this, OnAssignRightsMessage);
            _messageHub.Subscribe<BanUserWithDurationMessage>(this, OnBanUserWithDurationMessage);
            _messageHub.Subscribe<LetUserInMessage>(this, OnLetUserInMessage);
            _messageHub.Subscribe<MuteAllInRoomMessage>(this, OnMuteAllInRoomMessage);
            _messageHub.Subscribe<RemoveAllRightsMessage>(this, OnRemoveAllRightsMessage);
            _messageHub.Subscribe<RemoveRightsMessage>(this, OnRemoveRightsMessage);
            _messageHub.Subscribe<RoomUserKickMessage>(this, OnRoomUserKickMessage);
            _messageHub.Subscribe<RoomUserMuteMessage>(this, OnRoomUserMuteMessage);
            _messageHub.Subscribe<UnbanUserFromRoomMessage>(this, OnUnbanUserFromRoomMessage);
        }

        private void OnAmbassadorAlertMessage(AmbassadorAlertMessage message, ISession session)
        {
            if (session.Player == null) return;
        }

        private async Task OnAssignRightsMessage(AssignRightsMessage message, ISession session)
        {
            if (session.Player == null) return;

            await session.Player.RoomObject?.Room?.RoomSecurityManager?.AdjustRightsForPlayerId(session.Player, message.PlayerId, true);
        }

        private void OnBanUserWithDurationMessage(BanUserWithDurationMessage message, ISession session)
        {
            if (session.Player == null) return;
        }

        private void OnLetUserInMessage(LetUserInMessage message, ISession session)
        {
            if (session.Player == null) return;
        }

        private void OnMuteAllInRoomMessage(MuteAllInRoomMessage message, ISession session)
        {
            if (session.Player == null) return;
        }

        private async Task OnRemoveAllRightsMessage(RemoveAllRightsMessage message, ISession session)
        {
            await session.Player.RoomObject?.Room?.RoomSecurityManager?.RemoveAllRights(session.Player);
        }

        private async Task OnRemoveRightsMessage(RemoveRightsMessage message, ISession session)
        {
            if (session.Player == null) return;

            var roomSecurityManager = session.Player.RoomObject?.Room?.RoomSecurityManager;

            if (roomSecurityManager == null) return;

            foreach (var playerId in message.PlayerIds) await roomSecurityManager.AdjustRightsForPlayerId(session.Player, playerId, false);
        }

        private void OnRoomUserKickMessage(RoomUserKickMessage message, ISession session)
        {
            if (session.Player == null) return;
        }

        private void OnRoomUserMuteMessage(RoomUserMuteMessage message, ISession session)
        {
            if (session.Player == null) return;
        }

        private void OnUnbanUserFromRoomMessage(UnbanUserFromRoomMessage message, ISession session)
        {
            if (session.Player == null) return;
        }
    }
}