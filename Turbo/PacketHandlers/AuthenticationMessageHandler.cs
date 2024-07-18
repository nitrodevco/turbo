using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Events;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Core.Security;
using Turbo.Events.Game.Security;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Handshake;

namespace Turbo.Main.PacketHandlers
{
    public class AuthenticationMessageHandler : IAuthenticationMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly ITurboEventHub _eventHub;
        private readonly ISecurityManager _securityManager;
        private readonly IPlayerManager _playerManager;
        private readonly ILogger<AuthenticationMessageHandler> _logger;

        public AuthenticationMessageHandler(IPacketMessageHub messageHub, ISecurityManager securityManager, IPlayerManager playerManager, ILogger<AuthenticationMessageHandler> logger, ITurboEventHub eventHub)
        {
            _messageHub = messageHub;
            _securityManager = securityManager;
            _playerManager = playerManager;
            _logger = logger;
            _eventHub = eventHub;

            _messageHub.Subscribe<SSOTicketMessage>(this, OnSSOTicket);
            _messageHub.Subscribe<InfoRetrieveMessage>(this, OnInfoRetrieve);
        }

        public async Task OnSSOTicket(SSOTicketMessage message, ISession session)
        {
            int userId = await GetUserIdFromTicket(message);

            if (userId <= 0)
            {
                await session.DisposeAsync();
                return;
            }

            IPlayer player = await CreatePlayer(userId, session);

            if (player == null)
            {
                await session.DisposeAsync();
                return;
            }

            await SendRequiredComposersForHotelView(session);
            await DispatchUserLoginEvent(player);
        }

        private async Task<int> GetUserIdFromTicket(SSOTicketMessage message)
        {
            return await _securityManager.GetPlayerIdFromTicket(message.SSO);
        }

        private async Task<IPlayer> CreatePlayer(int userId, ISession session)
        {
            return await _playerManager.CreatePlayer(userId, session);
        }

        private async Task SendRequiredComposersForHotelView(ISession session)
        {
            await session.Send(new AuthenticationOKMessage());
            await session.Send(new UserRightsMessage
            {
                ClubLevel = ClubLevelEnum.Vip,
                SecurityLevel = SecurityLevelEnum.Moderator,
                IsAmbassador = false
            });
        }

        private async Task DispatchUserLoginEvent(IPlayer player)
        {
            var userLoginEvent = _eventHub.Dispatch(new UserLoginEvent
            {
                Player = player
            });

            if (userLoginEvent.IsCancelled)
            {
                await player.DisposeAsync();
            }
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