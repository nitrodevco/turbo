using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Core.Security;
using Turbo.Core.Security.Authentication;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Handshake;

namespace Turbo.Security.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly ISecurityManager _securityManager;
        private readonly IPlayerManager _playerManager;
        private readonly ILogger<IAuthenticationService> _logger;

        public AuthenticationService(IPacketMessageHub messageHub, ISecurityManager securityManager, IPlayerManager playerManager, ILogger<IAuthenticationService> logger)
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

            if (userId <= 0) return;

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
            await session.Send(new UserObjectMessage
            {
                Player = session.Player
            });
        }
    }
}
