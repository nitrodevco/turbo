using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Core.Security;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Navigator;

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
            await session.Send(new UserRightsMessage
            {
                ClubLevel = ClubLevelEnum.Vip,
                SecurityLevel = SecurityLevelEnum.Moderator,
                IsAmbassador = false
            });
            await session.Send(new NavigatorSettingsMessage
            {
                HomeRoomId = 0,
                RoomIdToEnter = 0
            });
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
