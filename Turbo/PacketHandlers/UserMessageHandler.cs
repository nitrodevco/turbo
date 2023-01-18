using System;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Inventory.Badges;
using Turbo.Packets.Incoming.Inventory.Furni;
using Turbo.Packets.Incoming.Users;
using Turbo.Packets.Outgoing.Users;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Game.Players;

namespace Turbo.Main.PacketHandlers
{
    public class UserMessageHandler : IUserMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IPlayerManager _playerManager;

        public UserMessageHandler(
            IPacketMessageHub messageHub,
            IPlayerManager playerManager)
        {
            _messageHub = messageHub;
            _playerManager = playerManager;

            _messageHub.Subscribe<GetSelectedBadgesMessage>(this, OnGetSelectedBadgesMessage);
        }

        protected virtual async void OnGetSelectedBadgesMessage(GetSelectedBadgesMessage message, ISession session)
        {
            if (session.Player == null) return;

            var activeBadges = await _playerManager.GetPlayerActiveBadges(message.PlayerId);

            await session.Send(new UserBadgesMessage
            {
                PlayerId = message.PlayerId,
                ActiveBadges = activeBadges
            });
        }
    }
}

