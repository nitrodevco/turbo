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
using Turbo.Networking.Game.Codec;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Availability;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Notifications;
using Turbo.Security;

namespace Turbo.Main.PacketHandlers;

public class AuthenticationMessageHandler(
    IPacketMessageHub messageHub,
    ISecurityManager securityManager,
    IPlayerManager playerManager,
    ILogger<AuthenticationMessageHandler> logger,
    ITurboEventHub eventHub,
    IDiffieService diffieService)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<InitDiffieHandshakeMessage>(this, OnHandshake);
        messageHub.Subscribe<CompleteDiffieHandshakeMessage>(this, OnCompleteHandshake);
        messageHub.Subscribe<SSOTicketMessage>(this, OnSSOTicket);
        messageHub.Subscribe<InfoRetrieveMessage>(this, OnInfoRetrieve);
    }

    private async void OnCompleteHandshake(CompleteDiffieHandshakeMessage message, ISession session)
    {
        var sharedKey = diffieService.GetSharedKey(message.SharedKey);

        session.Rc4 = new Rc4Service(sharedKey);

        logger.LogInformation("Diffie handshake completed for {0}", session.IPAddress);

        session.Channel.Pipeline.AddBefore("frameDecoder", "encryptionDecoder", new EncryptionDecoder(session));

        await session.Send(new CompleteDiffieHandshakeComposer
        {
            PublicKey = diffieService.GetPublicKey()
        });
    }

    private async void OnHandshake(InitDiffieHandshakeMessage message, ISession session)
    {
        // rsa
        var prime = diffieService.GetSignedPrime();
        var generator = diffieService.GetSignedGenerator();

        await session.Send(new InitDiffieHandshakeComposer
        {
            Prime = prime,
            Generator = generator
        });
    }

    private async Task OnSSOTicket(SSOTicketMessage message, ISession session)
    {
        var userId = await securityManager.GetPlayerIdFromTicket(message.SSO);

        if (userId <= 0)
        {
            await session.DisposeAsync();

            return;
        }

        var player = await playerManager.CreatePlayer(userId, session);

        if (player == null)
        {
            await session.DisposeAsync();

            return;
        }

        // send required composers for hotel view
        await session.Send(new AuthenticationOKMessage
        {
            AccountId = session.Player.Id,
            SuggestedLoginActions = [],
            IdentityId = session.Player.Id
        });

        await session.Send(new NavigatorSettingsMessage
        {
            HomeRoomId = 0,
            RoomIdToEnter = 0
        });

        await session.Send(new AvailabilityStatusMessage
        {
            IsOpen = true,
            OnShutDown = false,
            IsAuthenticHabbo = true
        });

        await session.Send(new InfoFeedEnableMessage
        {
            Enabled = true
        });

        await session.Send(new UserRightsMessage
        {
            ClubLevel = ClubLevelEnum.Vip,
            SecurityLevel = SecurityLevelEnum.Administrator,
            IsAmbassador = false
        });


        var messager = eventHub.Dispatch(new UserLoginEvent
        {
            Player = player
        });

        if (messager.IsCancelled) await player.DisposeAsync();
    }

    public async Task OnInfoRetrieve(InfoRetrieveMessage message, ISession session)
    {
        await session.Send(new UserObjectMessage
        {
            Player = session.Player
        });
    }
}