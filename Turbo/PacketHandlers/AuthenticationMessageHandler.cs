using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Core.Security;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Perks;

namespace Turbo.Main.PacketHandlers
{
    public class AuthenticationMessageHandler : IAuthenticationMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly ISecurityManager _securityManager;
        private readonly IPlayerManager _playerManager;
        private readonly ILogger<AuthenticationMessageHandler> _logger;

        public AuthenticationMessageHandler(IPacketMessageHub messageHub, ISecurityManager securityManager, IPlayerManager playerManager, ILogger<AuthenticationMessageHandler> logger)
        {
            _messageHub = messageHub;
            _securityManager = securityManager;
            _playerManager = playerManager;
            _logger = logger;

            _messageHub.Subscribe<SSOTicketMessage>(this, OnSSOTicket);
            _messageHub.Subscribe<InfoRetrieveMessage>(this, OnInfoRetrieve);
        }

        public async Task OnSSOTicket(SSOTicketMessage message, ISession session)
        {
            int userId = await _securityManager.GetPlayerIdFromTicket(message.SSO);

            if (userId <= 0)
            {
                await session.DisposeAsync();

                return;
            }

            IPlayer player = await _playerManager.CreatePlayer(userId, session);

            if (player == null)
            {
                await session.DisposeAsync();

                return;
            }

            // set player online
            // send required composers for hotel view
            await session.Send(new AuthenticationOKMessage());
        }

        public async Task OnInfoRetrieve(InfoRetrieveMessage message, ISession session)
        {
            await session.SendQueue(new UserObjectMessage
            {
                Player = session.Player
            });

            await session.SendQueue(new UserRightsMessage
            {
                ClubLevel = 2, // todo: remove hardcoded stuff
                IsAmbassador = session.Player.HasPermission("ambassador"),
                SecurityLevel = session.Player.PermissionComponent.Rank.ClientLevel
            });

            await session.SendQueue(new PerkAllowancesMessage
            {
                Perks = new List<Perk>() // todo: get actual perks
            });

            session.Flush();
        }
    }
}
